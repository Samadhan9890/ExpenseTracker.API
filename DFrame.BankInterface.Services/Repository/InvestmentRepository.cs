using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using ExpenseTracker.Services.Contracts.IContracts;
using ExpenseTracker.Services.Contracts.RequestResponseDto;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Models.ServiceModels;
using ExpenseTracker.Services.Repository.Generic;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;
using ExpenseTracker.Services.Utilities;
using ExpenseTracker.Services.Utilities.ConstantHelper;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using System.Net;
using System.Security.Claims;
using System.Text;
using Twilio.TwiML.Messaging;
using static ExpenseTracker.Services.Utilities.ConstantHelper.ConstantHelper;

namespace ExpenseTracker.Services.Repository
{
    public class InvestmentRepository : IInvestmentRepository
    {
        private readonly AppDBContext _appDbContext;
        private readonly ILogger<InvestmentRepository> _logger;
        private readonly IFeatureManager _featureManager;
        private readonly CommunicationConfigOptions _communicationConfigOptions;
        private readonly IMessageService _messageService;
        private readonly IBusinessDevTeamRepository _bdtRepo;
        private readonly IHttpContextAccessor _httpContextFactory;
        private readonly UserClaims userClaims;
        public InvestmentRepository(AppDBContext appDBContext,
            ILogger<InvestmentRepository> logger,
            IFeatureManager featureManager,
            IOptions<CommunicationConfigOptions> options
            , IMessageService messageService
            , IBusinessDevTeamRepository bdtRepo
            , IHttpContextAccessor httpContextFactory)
        {
            _appDbContext = appDBContext;
            _logger = logger;
            _featureManager = featureManager;
            _communicationConfigOptions = options.Value;
            _messageService = messageService;
            _bdtRepo = bdtRepo;
            _httpContextFactory = httpContextFactory;
            userClaims = UserClaimsHelper.GetClaims(_httpContextFactory.HttpContext.User.Identity as ClaimsIdentity);
        }

        public async Task DeleteInvestment(int investmentId)
        {
            try
            {
                var investment = await GetInvestmentByIdAsync(investmentId);
                if (investment != null)
                {
                    _appDbContext.Investments.Remove(investment);
                    await _appDbContext.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning($"Investment with ID {investmentId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting Investment with ID {investmentId} - {ex}");
                throw;
            }
        }

        public async Task<ClientMaster?> GetClientWithInvestmentByClientIdAsync(int clientId)
        {
            // Fetch the client with the given clientId
            var client = await _appDbContext.ClientMasters
                                 .AsNoTracking()
                                 .Where(c => c.ClientId == clientId)
                                 .FirstOrDefaultAsync();



            if (client != null)
            {
                // Populate investments and related details for the client
                await PopulateClientInvestmentDetailsAsync(client);
            }

            return client;
        }


        public async Task<Investment> GetInvestmentByIdAsync(int investmentId)
        {
            Investment? investment = await _appDbContext.Investments.AsNoTracking()
                                 .Where(c => c.InvestmentId == investmentId)
                                 .FirstOrDefaultAsync();
            var paymentReceivedDetails = await _appDbContext.InvestmentReceivedDetails.AsNoTracking()
                                            .Where(i => i.InvestmentId == investmentId).ToListAsync();

            var clientBanking = await _appDbContext.ClientBankingDetails.AsNoTracking()
                                            .Where(i => i.InvestmentId == investmentId).ToListAsync();

            investment.LstInvestmentReceivedDetails = paymentReceivedDetails;
            investment.LstClientBankingDetails = clientBanking;

            return investment;
        }

        public async Task<List<Investment>> GetAllInvestments()
        {
            List<Investment> invLst = await _appDbContext.Investments.AsNoTracking().ToListAsync();
            return invLst;
        }

        public async Task<Investment> CreateInvestmentAsync(InvestmentsDto investmentsDto)
        {
            try
            {
                // Step 1: Create the Investment record
                var investmentEntity = new Investment
                {
                    ClientId = investmentsDto.ClientId,
                    ClientName = investmentsDto.ClientName,
                    PlanId = investmentsDto.PlanId,
                    PlanName = investmentsDto.PlanName,
                    InvestmentAmount = investmentsDto.InvestmentAmount,
                    InvestmentStartDate = investmentsDto.InvestmentStartDate,
                    InvestmentEndDate = investmentsDto.InvestmentEndDate,
                    PayoutFrequency = investmentsDto.PayoutFrequency,
                    TotalProfitPercent = investmentsDto.TotalProfitPercent,
                    PayoutFrequencyProfitRatePercent = investmentsDto.PayoutFrequencyProfitRatePercent,
                    InvestmentTenure = investmentsDto.InvestmentTenure,
                    IsPaymentScheduleAvailable = investmentsDto.IsPaymentScheduleAvailable,
                    Guid = Guid.NewGuid(), // Generating new Guid
                    CreatedDate = DateTime.UtcNow, // Setting current time as created date
                    CreatedBy = investmentsDto.CreatedBy,
                    IsTdsApplicable = investmentsDto.IsTdsApplicable,
                    TdsPercent = investmentsDto.TdsPercent,
                    IsReferralBonusApplicable = investmentsDto.IsReferralBonusApplicable,
                    ReferralFirstBonusPercent = investmentsDto.ReferralFirstBonusPercent,
                    ReferralLastBonusPercent = investmentsDto.ReferralLastBonusPercent,
                    IsMaturityBonusApplicable = investmentsDto.IsMaturityBonusApplicable,
                    BonusTime = investmentsDto.BonusTime,
                    BonusPercent = investmentsDto.BonusPercent,
                    CapitalReturnPeriod = investmentsDto.CapitalReturnPeriod,
                    CapitalLockingReturnPeriod = investmentsDto.CapitalLockingReturnPeriod,
                    IsJoiningBonusApplicable = investmentsDto.IsJoiningBonusApplicable,
                    JoiningBonusPercent = investmentsDto.JoiningBonusPercent,
                    Status = 1
                };
                using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Save Investment record to generate InvestmentId and Guid
                        _appDbContext.Investments.Add(investmentEntity);
                        await _appDbContext.SaveChangesAsync();

                        // Step 2: Use generated InvestmentId and InvestmentGuid
                        var generatedInvestmentId = investmentEntity.InvestmentId; // Auto-incremented ID
                        var generatedInvestmentGuid = investmentEntity.Guid; // Auto-generated GUID

                        // Step 3: Save related ClientBankingDetails records (if present)
                        if (investmentsDto.LstClientBankingDetails != null && investmentsDto.LstClientBankingDetails.Any())
                        {
                            foreach (var bankingDetailDto in investmentsDto.LstClientBankingDetails)
                            {
                                var bankingDetailEntity = new ClientBankingDetail
                                {
                                    ClientId = investmentsDto.ClientId,
                                    InvestmentId = generatedInvestmentId,
                                    InvestmentGuid = generatedInvestmentGuid,
                                    Mode = bankingDetailDto.Mode,
                                    AccountNoOrUpiId = bankingDetailDto.AccountNoOrUpiId,
                                    BankName = bankingDetailDto.BankName,
                                    AccountHolderName = bankingDetailDto.AccountHolderName,
                                    IFSCCode = bankingDetailDto.IFSCCode,
                                    Status = bankingDetailDto.Status,
                                    CreatedBy = investmentsDto.CreatedBy,
                                    BankingType = bankingDetailDto.BankingType ?? "Client",
                                    Note = bankingDetailDto.Note
                                };

                                _appDbContext.ClientBankingDetails.Add(bankingDetailEntity);
                            }
                            await _appDbContext.SaveChangesAsync();
                        }

                        // Step 4: Save related InvestmentReceivedDetails records (if present)
                        if (investmentsDto.LstInvestmentReceivedDetails != null && investmentsDto.LstInvestmentReceivedDetails.Any())
                        {
                            foreach (var receivedDetailDto in investmentsDto.LstInvestmentReceivedDetails)
                            {
                                var receivedDetailEntity = new InvestmentReceivedDetails
                                {
                                    InvestmentId = generatedInvestmentId,
                                    InvestmentGuid = generatedInvestmentGuid,
                                    Mode = receivedDetailDto.Mode,
                                    BankName = receivedDetailDto.BankName,
                                    AccountNumberOrUpiId = receivedDetailDto.AccountNumberOrUpiId,
                                    Amount = receivedDetailDto.Amount,
                                    Comments = receivedDetailDto.Comments,
                                    AddedBy = investmentsDto.CreatedBy,
                                    ReceivedDate = receivedDetailDto.ReceivedDate,
                                    Status = true
                                };

                                _appDbContext.InvestmentReceivedDetails.Add(receivedDetailEntity);
                            }
                            await _appDbContext.SaveChangesAsync();

                            // Commit transaction for these operations
                            await transaction.CommitAsync();

                            // Add entry in Audit Trail
                            var genfunc = new GenericFunctions(_appDbContext);
                            await genfunc.AddNcAuditTrail(investmentsDto.ClientId,
                                investmentEntity.InvestmentId,
                                null,
                                nameof(ConstantHelper.NCAuditModulesEnum.SPL_INVESTMENTS),
                                $"New Investment created for - {investmentsDto.ClientId}, Plan - {investmentsDto.PlanName}, Amount - INR{investmentsDto.InvestmentAmount}",
                                null,
                                investmentsDto.CreatedBy);


                        }
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if any error occurs
                        await transaction.RollbackAsync();
                        _logger.LogError($"Transaction failed: Unable to create new investment {ex}");
                        throw;
                    }

                    await _appDbContext.SaveChangesAsync();
                }
                return investmentEntity;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occured while adding new Investment - {ex}");
                throw;
            }

        }
        public async Task<Investment> UpdateInvestmentAsync(UpdateInvestmentsDto investmentsDto)
        {
            try
            {
                var investmentEntity = await _appDbContext.Investments.FirstOrDefaultAsync(i => i.InvestmentId == investmentsDto.InvestmentId);

                if (investmentEntity == null)
                {
                    throw new Exception("Investment not found.");
                }

                // Update only changed properties
                if (investmentEntity.ClientName != investmentsDto.ClientName)
                {
                    investmentEntity.ClientName = investmentsDto.ClientName;
                }

                if (investmentEntity.PlanId != investmentsDto.PlanId)
                {
                    investmentEntity.PlanId = investmentsDto.PlanId;
                }

                if (investmentEntity.PlanName != investmentsDto.PlanName)
                {
                    investmentEntity.PlanName = investmentsDto.PlanName;
                }

                if (investmentEntity.InvestmentAmount != investmentsDto.InvestmentAmount)
                {
                    investmentEntity.InvestmentAmount = investmentsDto.InvestmentAmount;
                }

                if (investmentEntity.InvestmentStartDate != investmentsDto.InvestmentStartDate)
                {
                    investmentEntity.InvestmentStartDate = investmentsDto.InvestmentStartDate;
                }

                if (investmentEntity.InvestmentEndDate != investmentsDto.InvestmentEndDate)
                {
                    investmentEntity.InvestmentEndDate = investmentsDto.InvestmentEndDate;
                }

                if (investmentEntity.PayoutFrequency != investmentsDto.PayoutFrequency)
                {
                    investmentEntity.PayoutFrequency = investmentsDto.PayoutFrequency;
                }

                if (investmentEntity.TotalProfitPercent != investmentsDto.TotalProfitPercent)
                {
                    investmentEntity.TotalProfitPercent = investmentsDto.TotalProfitPercent;
                }

                if (investmentEntity.PayoutFrequencyProfitRatePercent != investmentsDto.PayoutFrequencyProfitRatePercent)
                {
                    investmentEntity.PayoutFrequencyProfitRatePercent = investmentsDto.PayoutFrequencyProfitRatePercent;
                }

                if (investmentEntity.InvestmentTenure != investmentsDto.InvestmentTenure)
                {
                    investmentEntity.InvestmentTenure = investmentsDto.InvestmentTenure;
                }

                if (investmentEntity.IsPaymentScheduleAvailable != investmentsDto.IsPaymentScheduleAvailable)
                {
                    investmentEntity.IsPaymentScheduleAvailable = investmentsDto.IsPaymentScheduleAvailable;
                }

                if (investmentEntity.IsTdsApplicable != investmentsDto.IsTdsApplicable)
                {
                    investmentEntity.IsTdsApplicable = investmentsDto.IsTdsApplicable;
                }

                if (investmentEntity.TdsPercent != investmentsDto.TdsPercent)
                {
                    investmentEntity.TdsPercent = investmentsDto.TdsPercent;
                }

                if (investmentEntity.IsReferralBonusApplicable != investmentsDto.IsReferralBonusApplicable)
                {
                    investmentEntity.IsReferralBonusApplicable = investmentsDto.IsReferralBonusApplicable;
                }

                if (investmentEntity.ReferralFirstBonusPercent != investmentsDto.ReferralFirstBonusPercent)
                {
                    investmentEntity.ReferralFirstBonusPercent = investmentsDto.ReferralFirstBonusPercent;
                }

                if (investmentEntity.ReferralLastBonusPercent != investmentsDto.ReferralLastBonusPercent)
                {
                    investmentEntity.ReferralLastBonusPercent = investmentsDto.ReferralLastBonusPercent;
                }

                if (investmentEntity.IsMaturityBonusApplicable != investmentsDto.IsMaturityBonusApplicable)
                {
                    investmentEntity.IsMaturityBonusApplicable = investmentsDto.IsMaturityBonusApplicable;
                }

                if (investmentEntity.BonusTime != investmentsDto.BonusTime)
                {
                    investmentEntity.BonusTime = investmentsDto.BonusTime;
                }

                if (investmentEntity.BonusPercent != investmentsDto.BonusPercent)
                {
                    investmentEntity.BonusPercent = investmentsDto.BonusPercent;
                }

                if (investmentEntity.CapitalReturnPeriod != investmentsDto.CapitalReturnPeriod)
                {
                    investmentEntity.CapitalReturnPeriod = investmentsDto.CapitalReturnPeriod;
                }

                if (investmentEntity.CapitalLockingReturnPeriod != investmentsDto.CapitalLockingReturnPeriod)
                {
                    investmentEntity.CapitalLockingReturnPeriod = investmentsDto.CapitalLockingReturnPeriod;
                }

                if (investmentEntity.IsJoiningBonusApplicable != investmentsDto.IsJoiningBonusApplicable)
                {
                    investmentEntity.IsJoiningBonusApplicable = investmentsDto.IsJoiningBonusApplicable;
                }

                if (investmentEntity.JoiningBonusPercent != investmentsDto.JoiningBonusPercent)
                {
                    investmentEntity.JoiningBonusPercent = investmentsDto.JoiningBonusPercent;
                }

                // Save changes only if any field has changed.Entity Framework will only send SQL updates for the properties that were modified.
                await _appDbContext.SaveChangesAsync();

                // Add entry in Audit Trail
                var genfunc = new GenericFunctions(_appDbContext);
                await genfunc.AddNcAuditTrail(investmentsDto.ClientId,
                    investmentEntity.InvestmentId,
                    null,
                    nameof(ConstantHelper.NCAuditModulesEnum.SPL_INVESTMENTS),
                    $"Investment Details has been updated - {investmentsDto.ClientId}, Plan - {investmentsDto.PlanName}, Amount - INR{investmentsDto.InvestmentAmount}",
                    null,
                    userClaims.UserLoginCode); //this need to be updated as per modified by

                return investmentEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating Investment - {ex}");
                throw;
            }
        }
        public async Task<InvestmentReceivedDetails> UpdateInvestmentReceivedDetailsAsync(InvestmentReceivedDetailDto receivedDetailDto)
        {
            try
            {
                var receivedDetailEntity = await _appDbContext.InvestmentReceivedDetails.FirstOrDefaultAsync(i => i.Id == receivedDetailDto.Id);

                if (receivedDetailEntity == null)
                {
                    throw new Exception("InvestmentReceivedDetails not found.");
                }

                // Update only changed properties
                if (receivedDetailEntity.Mode != receivedDetailDto.Mode)
                {
                    receivedDetailEntity.Mode = receivedDetailDto.Mode;
                }
                if (receivedDetailEntity.BankName != receivedDetailDto.BankName)
                {
                    receivedDetailEntity.BankName = receivedDetailDto.BankName;
                }

                if (receivedDetailEntity.Amount != receivedDetailDto.Amount)
                {
                    receivedDetailEntity.Amount = receivedDetailDto.Amount;
                }

                if (receivedDetailEntity.Comments != receivedDetailDto.Comments)
                {
                    receivedDetailEntity.Comments = receivedDetailDto.Comments;
                }
                if (receivedDetailEntity.InvestmentAttachment != receivedDetailDto.InvestmentAttachment)
                {
                    receivedDetailEntity.InvestmentAttachment = receivedDetailDto.InvestmentAttachment;
                }
                // Add other field comparisons here if there are more fields to update

                // Save changes only if any field has changed
                await _appDbContext.SaveChangesAsync();

                var clientid = _appDbContext.Investments.AsNoTracking()
                    .Where(i => i.InvestmentId == receivedDetailEntity.InvestmentId).FirstOrDefaultAsync().GetAwaiter().GetResult().ClientId;

                var genfunc = new GenericFunctions(_appDbContext);
                await genfunc.AddNcAuditTrail(clientid,
                    receivedDetailEntity.InvestmentId,
                    null,
                    nameof(ConstantHelper.NCAuditModulesEnum.SPL_INVESTMENTS),
                    $"Investment received details has been updated for {receivedDetailEntity.InvestmentId}; Amount - {receivedDetailEntity.Amount}; Received Date - {receivedDetailEntity.ReceivedDate};",
                    null,
                    userClaims.UserLoginCode);

                return receivedDetailEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating InvestmentReceivedDetails - {ex}");
                throw;
            }
        }

        public async Task<ClientBankingDetail> UpdateClientBankingDetailAsync(ClientBankingDetailsDto bankingDetailDto)
        {
            try
            {
                var bankingDetailEntity = await _appDbContext.ClientBankingDetails.FirstOrDefaultAsync(i => i.Id == bankingDetailDto.Id);

                if (bankingDetailEntity == null)
                {
                    throw new Exception("ClientBankingDetail not found.");
                }

                // Update only changed properties
                if (bankingDetailEntity.BankName != bankingDetailDto.BankName)
                {
                    bankingDetailEntity.BankName = bankingDetailDto.BankName;
                }

                if (bankingDetailEntity.AccountHolderName != bankingDetailDto.AccountHolderName)
                {
                    bankingDetailEntity.AccountHolderName = bankingDetailDto.AccountHolderName;
                }

                if (bankingDetailEntity.IFSCCode != bankingDetailDto.IFSCCode)
                {
                    bankingDetailEntity.IFSCCode = bankingDetailDto.IFSCCode;
                }

                if (bankingDetailEntity.AccountNoOrUpiId != bankingDetailDto.AccountNoOrUpiId)
                {
                    bankingDetailEntity.AccountNoOrUpiId = bankingDetailDto.AccountNoOrUpiId;
                }

                if (bankingDetailEntity.Mode != bankingDetailDto.Mode)
                {
                    bankingDetailEntity.Mode = bankingDetailDto.Mode;
                }

                if (bankingDetailEntity.Status != bankingDetailDto.Status)
                {
                    bankingDetailEntity.Status = bankingDetailDto.Status;
                }

                if (bankingDetailEntity.Note != bankingDetailDto.Note)
                {
                    bankingDetailEntity.Note = bankingDetailDto.Note;
                }

                // Check if BankingType is empty or null, set default to "Client"
                var bankingTypeToSet = string.IsNullOrWhiteSpace(bankingDetailDto.BankingType)
                    ? "Client"
                    : bankingDetailDto.BankingType;

                if (bankingDetailEntity.BankingType != bankingTypeToSet)
                {
                    bankingDetailEntity.BankingType = bankingTypeToSet;
                }

                // Save changes only if any field has changed
                await _appDbContext.SaveChangesAsync();

                var clientid = _appDbContext.Investments.AsNoTracking()
                   .Where(i => i.InvestmentId == bankingDetailDto.InvestmentId).FirstOrDefaultAsync().GetAwaiter().GetResult().ClientId;

                var genfunc = new GenericFunctions(_appDbContext);
                await genfunc.AddNcAuditTrail(clientid,
                    bankingDetailDto.InvestmentId,
                    null,
                    nameof(ConstantHelper.NCAuditModulesEnum.SPL_INVESTMENTS),
                    $"Clients Banking details has been updated for {bankingDetailDto.InvestmentId}; Account No - {bankingDetailDto.AccountNoOrUpiId};",
                    null,
                    userClaims.UserLoginCode);

                return bankingDetailEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating ClientBankingDetail - {ex}");
                throw;
            }
        }
        public async Task<Dictionary<int, string>> GetClientNamesByIdsAsync(List<int> clientIds)
        {
            try
            {
                _logger.LogInformation("Fetching ClientNames for ClientIds.");

                var clientNames = await _appDbContext.ClientMasters
                                        .Where(c => clientIds.Contains(c.ClientId))
                                        .ToDictionaryAsync(c => c.ClientId, c => c.Name);

                _logger.LogInformation($"Fetched {clientNames.Count} client names.");
                return clientNames;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching client names.");
                throw;
            }
        }

        public async Task<List<SplPaymentSchedule>> SaveInvestmentPaymentSchedulesAsync(List<SplPaymentSchedule> schedules)
        {
            try
            {
                // Adding the entities to the DbContext
                await _appDbContext.SplPaymentSchedules.AddRangeAsync(schedules);

                //set payment status as available
                _appDbContext.Investments.Where(i => i.InvestmentId == schedules.First().InvestmentId).FirstOrDefaultAsync().GetAwaiter().GetResult().IsPaymentScheduleAvailable = true;

                // Save changes to the database
                await _appDbContext.SaveChangesAsync();

                _logger.LogInformation("Successfully saved investment payment schedules.");

                // After SaveChangesAsync, the primary keys will be populated in the entities

                StringBuilder message = new StringBuilder();
                message.Append("Payment Schedule Created | ");
                message.Append($"Joining Bonus - {schedules.Where(s => s.PaymentType == (int)SplPaymentTypesEnum.JoiningBonus).Sum(a => a.PayableAmount)} | ");
                var refDetails = schedules.Where(s => s.PaymentType == (int)SplPaymentTypesEnum.ReferralBonus).ToList();
                if (refDetails.Count > 0)
                {
                    message.Append($"Referral to {refDetails[0].ClientName} - {refDetails.Sum(a => a.PayableAmount)} | ");
                }
                message.Append($"Profit - {schedules.Where(s => s.PaymentType == (int)SplPaymentTypesEnum.ClientProfit).Sum(a => a.PayableAmount)} | ");
                message.Append($"Maturity Bonus - {schedules.Where(s => s.PaymentType == (int)SplPaymentTypesEnum.MaturityBonus).Sum(a => a.PayableAmount)} | ");


                var genfunc = new GenericFunctions(_appDbContext);
                await genfunc.AddNcAuditTrail(schedules[0].ClientId,
                    schedules[0].InvestmentId,
                    null,
                    nameof(ConstantHelper.NCAuditModulesEnum.SPL_INVESTMENTS),
                    message.ToString(),
                    null,
                    userClaims.UserLoginCode);

                return schedules; // Return the entities including the newly generated primary keys
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving investment payment schedules.");
                throw;
            }
        }

        public async Task<List<SplPaymentSchedule>> GetSplPaymentScheduleByInvestmentId(int investmentId)
        {
            var schedules = await _appDbContext.SplPaymentSchedules.AsNoTracking().Where(p => p.InvestmentId == investmentId).ToListAsync();
            return schedules;
        }
        public async Task<List<SplPaymentScheduleDto>> GetAllTodaysPaymentsFromInvestPayScheduleAsync()
        {
            // Get the current date
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                // Retrieve all todays payments with due date equal to current date and status equals 0
                var todaysPayments = await _appDbContext.SplPaymentSchedules.AsNoTracking()
                     .Where(p => p.DueDate == currentDate && (p.Status == 0 || p.Status == 1 || p.Status == 2))
                    .Select(dto => new SplPaymentScheduleDto
                    {
                        ScheduleId = dto.ScheduleId,
                        Guid = dto.Guid,
                        ClientId = dto.ClientId,
                        InvestmentId = dto.InvestmentId,
                        PaymentType = dto.PaymentType,
                        DueDate = dto.DueDate,
                        ProfitAmount = dto.ProfitAmount,
                        Tds = dto.Tds,
                        PayableAmount = dto.PayableAmount,
                        AmountPaid = dto.AmountPaid,
                        PaymentDate = dto.PaymentDate,
                        PaymentMode = dto.PaymentMode,
                        PaymentUtr = dto.PaymentUtr,
                        PaymentProofAttachment = dto.PaymentProofAttachment,
                        Status = dto.Status,
                        Notes = dto.Notes,
                        CreatedDate = dto.CreatedDate,
                        CreatedBy = dto.CreatedBy,
                        InterestRate = dto.InterestRate,
                        InvestedAmount = dto.InvestedAmount,
                        Day = dto.Day,
                        ClientName = dto.ClientName,
                        PaidBy = dto.PaidBy
                    }).OrderBy(p => p.DueDate)
                        .ToListAsync();

                return todaysPayments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting todays investment payment schedules.");
                throw;
            }
        }
        public async Task<List<SplPaymentScheduleDto>> GetAllDuedSplPaymentsFromInvestmentPayScheduleAsync()
        {
            // Get the current date
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            try
            {
                // Retrieve all due payments with due date less than current date and status equals 0
                var duedPayments = await _appDbContext.SplPaymentSchedules.AsNoTracking()
                .Where(p => p.DueDate < currentDate && (p.Status == 0 || p.Status == 1 || p.Status == 2))
                .Select(dto => new SplPaymentScheduleDto
                {
                    ScheduleId = dto.ScheduleId,
                    Guid = dto.Guid,
                    ClientId = dto.ClientId,
                    InvestmentId = dto.InvestmentId,
                    PaymentType = dto.PaymentType,
                    DueDate = dto.DueDate,
                    ProfitAmount = dto.ProfitAmount,
                    Tds = dto.Tds,
                    PayableAmount = dto.PayableAmount,
                    AmountPaid = dto.AmountPaid,
                    PaymentDate = dto.PaymentDate,
                    PaymentMode = dto.PaymentMode,
                    PaymentUtr = dto.PaymentUtr,
                    PaymentProofAttachment = dto.PaymentProofAttachment,
                    Status = dto.Status,
                    Notes = dto.Notes,
                    CreatedDate = dto.CreatedDate,
                    CreatedBy = dto.CreatedBy,
                    InterestRate = dto.InterestRate,
                    InvestedAmount = dto.InvestedAmount,
                    Day = dto.Day,
                    ClientName = dto.ClientName,
                    PaidBy = dto.PaidBy
                }).OrderBy(p => p.DueDate)
                    .ToListAsync();

                return duedPayments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting dued investment payment schedules.");
                throw;
            }
        }
        public async Task<List<SplPaymentScheduleDto>> GetInvestmentPaymentHistoriesAsync(DateOnly startDate, DateOnly endDate)
        {
            try
            {
                var paymentsToProcess = await _appDbContext.SplPaymentSchedules
                    .AsNoTracking()
                    .Where(ps => ps.DueDate > startDate && ps.DueDate <= endDate)
                   .Select(dto => new SplPaymentScheduleDto
                   {
                       ScheduleId = dto.ScheduleId,
                       Guid = dto.Guid,
                       ClientId = dto.ClientId,
                       InvestmentId = dto.InvestmentId,
                       PaymentType = dto.PaymentType,
                       DueDate = dto.DueDate,
                       ProfitAmount = dto.ProfitAmount,
                       Tds = dto.Tds,
                       PayableAmount = dto.PayableAmount,
                       AmountPaid = dto.AmountPaid,
                       PaymentDate = dto.PaymentDate,
                       PaymentMode = dto.PaymentMode,
                       PaymentUtr = dto.PaymentUtr,
                       PaymentProofAttachment = dto.PaymentProofAttachment,
                       Status = dto.Status,
                       Notes = dto.Notes,
                       CreatedDate = dto.CreatedDate,
                       CreatedBy = dto.CreatedBy,
                       InterestRate = dto.InterestRate,
                       InvestedAmount = dto.InvestedAmount,
                       Day = dto.Day,
                       ClientName = dto.ClientName,
                       PaidBy = dto.PaidBy
                   }).OrderBy(p => p.DueDate)
                        .ToListAsync();

                return paymentsToProcess;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting investment payment histories.");
                throw;
            }
        }

        public async Task<List<SplPaymentScheduleDto>> GetAllInvestmentPaymentsToProcessAsync()
        {
            try
            {
                var paymentsToProcess = await (from schedule in _appDbContext.SplPaymentSchedules.AsNoTracking()
                                               join banking in _appDbContext.ClientBankingDetails.AsNoTracking()
                                               on schedule.ClientId equals banking.ClientId // Join based on ClientId
                                               where schedule.Status == 1
                                               //schedule.InvestmentId == banking.InvestmentId
                                               //&& schedule.Status == 1 // Filter by status
                                               select new { schedule, banking })
                                      .ToListAsync();

                // Group and project into SplPaymentScheduleDto
                var paymentSchedules = paymentsToProcess
                    .GroupBy(x => new
                    {
                        x.schedule.ScheduleId,
                        x.schedule.Guid,
                        x.schedule.ClientId,
                        x.schedule.DueDate,
                        x.schedule.PayableAmount,
                        x.schedule.AmountPaid,
                        x.schedule.PaymentDate,
                        x.schedule.PaymentUtr,
                        x.schedule.PaymentMode,
                        x.schedule.PaidBy,
                        x.schedule.PaymentProofAttachment,
                        x.schedule.Status,
                        x.schedule.Notes,
                        x.schedule.CreatedDate,
                        x.schedule.CreatedBy,
                        x.schedule.InterestRate,
                        x.schedule.InvestedAmount,
                        x.schedule.Day,
                        x.schedule.ClientName,
                        x.schedule.InvestmentId,
                        x.schedule.ProfitAmount,
                        x.schedule.Tds,
                        x.schedule.PaymentType,

                    })
                    .Select(g => new SplPaymentScheduleDto
                    {
                        ScheduleId = g.Key.ScheduleId,
                        Guid = g.Key.Guid,
                        ClientId = g.Key.ClientId,
                        DueDate = g.Key.DueDate,
                        PayableAmount = g.Key.PayableAmount,
                        AmountPaid = g.Key.AmountPaid,
                        PaymentDate = g.Key.PaymentDate,
                        PaymentUtr = g.Key.PaymentUtr,
                        PaymentMode = g.Key.PaymentMode,
                        PaidBy = g.Key.PaidBy,
                        PaymentProofAttachment = g.Key.PaymentProofAttachment,
                        Status = g.Key.Status,
                        Notes = g.Key.Notes,
                        CreatedDate = g.Key.CreatedDate,
                        CreatedBy = g.Key.CreatedBy,
                        InterestRate = g.Key.InterestRate,
                        InvestedAmount = g.Key.InvestedAmount,
                        Day = g.Key.Day,
                        ClientName = g.Key.ClientName,
                        InvestmentId = g.Key.InvestmentId,
                        PaymentType = g.Key.PaymentType,
                        ProfitAmount = g.Key.ProfitAmount,
                        Tds = g.Key.Tds,

                        BankingDetails = g.Where(d => d.banking.Status = true).Select(x => new ClientBankingDetailsDto
                        {
                            Id = x.banking.Id,
                            AccountNoOrUpiId = x.banking.AccountNoOrUpiId,
                            BankName = x.banking.BankName,
                            AccountHolderName = x.banking.AccountHolderName,
                            IFSCCode = x.banking.IFSCCode,
                            Mode = x.banking.Mode,
                            Status = x.banking.Status,
                            InvestmentId = x.banking.InvestmentId,
                            ClientId = x.banking.ClientId,
                            InvestmentGuid = x.banking.InvestmentGuid,
                            CreatedBy = x.banking.CreatedBy,
                            CreatedDate = x.banking.CreatedDate,
                            Note = x.banking.Note
                        }).ToList()
                    })
                    .OrderBy(p => p.DueDate) // Ordering by DueDate
                    .ToList();

                // Step 3: Remove the banking details for records where PaymentType = 2
                //foreach (var schedule in paymentSchedules.Where(s => s.PaymentType == 2))
                //{
                //    schedule.BankingDetails.Clear(); // Clear banking details for PaymentType = 2
                //}

                //var paymentType2ClientIds = paymentSchedules
                //    .Where(s => s.PaymentType == 2)
                //    .Select(s => s.ClientId)
                //    .Distinct()
                //    .ToList();

                //// Step 4: Fetch the updated banking details for PaymentType == 2
                //var bankingDetailsForPaymentType2 = await (from client in _appDbContext.ClientMasters.AsNoTracking()
                //                                           join banking in _appDbContext.ClientBankingDetails.AsNoTracking()
                //                                           // Cast both fields to a common type, assuming they are not already the same
                //                                           on Convert.ToInt32(client.ReferredBy) equals (int?)banking.ClientId
                //                                           where paymentType2ClientIds.Contains(client.ClientId) // Match relevant ClientIds
                //                                                 && banking.BankingType == "Referral" // Condition: BankingType = "Referral"
                //                                           select new { client.ClientId, banking })
                //                                          .ToListAsync();


                //// Step 5: Update paymentSchedules with the new banking details for PaymentType == 2
                //foreach (var record in bankingDetailsForPaymentType2)
                //{
                //    // Find the matching schedule in paymentSchedules for PaymentType = 2
                //    var scheduleToUpdate = paymentSchedules.FirstOrDefault(s => s.ClientId == record.ClientId && s.PaymentType == 2);
                //    if (scheduleToUpdate != null)
                //    {
                //        // Clear the current banking details (if any) and add the new ones
                //        scheduleToUpdate.BankingDetails.Clear();
                //        scheduleToUpdate.BankingDetails.Add(new ClientBankingDetailsDto
                //        {
                //            Id = record.banking.Id,
                //            AccountNoOrUpiId = record.banking.AccountNoOrUpiId,
                //            BankName = record.banking.BankName,
                //            AccountHolderName = record.banking.AccountHolderName,
                //            IFSCCode = record.banking.IFSCCode,
                //            Mode = record.banking.Mode,
                //            Status = record.banking.Status,
                //            InvestmentId = record.banking.InvestmentId,
                //            ClientId = record.banking.ClientId,
                //            InvestmentGuid = record.banking.InvestmentGuid,
                //            CreatedBy = record.banking.CreatedBy,
                //            CreatedDate = record.banking.CreatedDate,
                //            BusinessDevTeamId = record.banking.BusinessDevTeamId,
                //        });
                //    }
                //}

                return paymentSchedules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting investment payment to process.");
                throw;
            }
        }
        public async Task<List<ExternalPayment>> GetAllExternalPaymentsAsync()
        {
            try
            {
                return await _appDbContext.ExternalPayments.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all external payments.");
                throw;
            }
        }
        public async Task<ExternalPayment> GetExternalPaymentsByIdAsync(int id)
        {
            try
            {
                return await _appDbContext.ExternalPayments.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching external payment by ID: {Id}", id);
                throw;
            }
        }

        public async Task<ExternalPayment> AddExternalPaymentsAsync(ExternalPayment payment)
        {
            try
            {
                _appDbContext.ExternalPayments.Add(payment);
                await _appDbContext.SaveChangesAsync();
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new external payment.");
                throw;
            }
        }

        public async Task<ExternalPayment> UpdateExternalPaymentsAsync(ExternalPayment payment)
        {
            try
            {
                _appDbContext.ExternalPayments.Update(payment);
                await _appDbContext.SaveChangesAsync();
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating external payment.");
                throw;
            }
        }

        public async Task DeleteExternalPaymentsAsync(int id)
        {
            try
            {
                var payment = await _appDbContext.ExternalPayments.FindAsync(id);
                if (payment != null)
                {
                    _appDbContext.ExternalPayments.Remove(payment);
                    await _appDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting external payment with ID: {Id}", id);
                throw;
            }
        }

        public async Task UpdateMakerInvestmentPaymentScheduleStatusAsync(List<Guid> payments, int statusToUpdate, string comments, string updatedBy)
        {
            var paySchedules = await _appDbContext.SplPaymentSchedules.Where(p => payments.Contains(p.Guid)).ToListAsync();

            paySchedules.ForEach(p => { p.Status = statusToUpdate; p.Notes = $"Maker - {comments} |"; });

            await _appDbContext.SaveChangesAsync();

            //create audit trail
            var genfunc = new GenericFunctions(_appDbContext);
            var auditTrails = new List<AuditTrail>();

            //add audit trail
            foreach (var pay in paySchedules)
            {
                await genfunc.AddNcAuditTrail(
                     pay.ClientId,
                     pay.InvestmentId,
                     pay.ScheduleId,
                     nameof(NCAuditModulesEnum.SPL_PAYMENTS),
                     $"Maker : Investment Payment Status updated to - {((PaymentStatusEnum)statusToUpdate).ToString()}",
                     comments.Substring(0, comments.Length <= 100 ? comments.Length : 100),
                     updatedBy
                 );
            }


            //send notification to approver
            if (_featureManager.IsEnabledAsync("SendPaymentSentForProcess").GetAwaiter().GetResult() && statusToUpdate == (int)PaymentStatusEnum.SentForProcessing)
            {
                StringBuilder sb = new();
                decimal amountSum = paySchedules.Sum(p => p.PayableAmount) ?? 0;
                sb.Append(_communicationConfigOptions.MessageTemplates.Where(m => m.MessageType == "PaymentSentForProcess").FirstOrDefault().Message);
                sb.Replace("{NoOfReceipients}", $"{paySchedules.Count}");
                sb.Replace("{totalPayments}", $"INR {amountSum}");
                sb.Replace("{comments}", comments);
                string receiverNumbers = _communicationConfigOptions.Contacts.Where(c => c.Type == "ApproverMobile").FirstOrDefault().ContactNo;
                await _messageService.SendMessage(sb.ToString(), receiverNumbers);
            }


        }

        public async Task UpdateManualInvestmentPaymentScheduleStatusAsync(UpdateManualInvestmentPaymentStatusDto payment)
        {
            var schedule = await _appDbContext.SplPaymentSchedules.Where(p => p.Guid == payment.Guid).FirstOrDefaultAsync();

            schedule.Status = payment.Status;
            schedule.Notes = schedule.Notes + "Processing Details -" + payment.BankingDetails; //string.IsNullOrWhiteSpace(payment.Comments) ? schedule.Notes : payment.Comments;
            schedule.PaymentUtr = string.IsNullOrWhiteSpace(payment.PaymentUtr) ? schedule.PaymentUtr : payment.PaymentUtr;
            schedule.PaymentDate = DateTime.Now;
            schedule.PaymentMode = "Manual";
            schedule.AmountPaid = schedule.PayableAmount;
            schedule.PaidBy = payment.PaidBy;

            await _appDbContext.SaveChangesAsync();

            //send notification to customer
            if (_featureManager.IsEnabledAsync("PaymentComplete").GetAwaiter().GetResult())
            {
                var client = await GetClientDetailByInvestmentPaymentId(payment.Guid);
                string cName = client != null ? client.Name : "Investor";

                StringBuilder sb = new();
                sb.Append(_communicationConfigOptions.MessageTemplates.Where(m => m.MessageType == "PaymentComplete").FirstOrDefault().Message);
                sb.Replace("{nameOfInvestor}", $"{cName}");
                sb.Replace("{amount paid}", $"{schedule.PayableAmount}");

                string receiverNumbers = client.Mobile;
                await _messageService.SendMessage(sb.ToString(), receiverNumbers);
            }

            string auditMessage = string.Empty;

            if (schedule.PaymentType == (int)SplPaymentTypesEnum.ClientProfit)
            {
                auditMessage = $"Manual Payment for Profit to {schedule.ClientName} completed.";
            }
            else if (schedule.PaymentType == (int)SplPaymentTypesEnum.JoiningBonus)
            {
                auditMessage = $"Manual Payment for Joining bonus to {schedule.ClientName} completed.";
            }
            else if (schedule.PaymentType == (int)SplPaymentTypesEnum.ReferralBonus)
            {
                auditMessage = $"Manual Payment for Referral bonus to {schedule.ClientName} completed.";
            }
            else if (schedule.PaymentType == (int)SplPaymentTypesEnum.MaturityBonus)
            {
                auditMessage = $"Manual Payment for Maturity bonus to {schedule.ClientName} completed.";
            }
            else if (schedule.PaymentType == (int)SplPaymentTypesEnum.CapitalReturn)
            {
                auditMessage = $"Manual Payment for Capital Return to {schedule.ClientName} completed.";
            }

            var genfunc = new GenericFunctions(_appDbContext);
            await genfunc.AddNcAuditTrail(schedule.ClientId,
                schedule.InvestmentId,
                schedule.ScheduleId,
                nameof(ConstantHelper.NCAuditModulesEnum.SPL_PAYMENTS),
                $"{auditMessage}  - {payment.BankingDetails}",
                null,
                schedule.PaidBy);

        }

        public async Task<ClientMaster> GetClientDetailByInvestmentPaymentId(Guid guid)
        {
            var clientId = _appDbContext.SplPaymentSchedules.AsNoTracking().Where(p => p.Guid == guid).FirstOrDefaultAsync().GetAwaiter().GetResult().ClientId;
            var clientDetails = await _appDbContext.ClientMasters.Where(c => c.ClientId == clientId).FirstOrDefaultAsync();
            return clientDetails;
        }



        public async Task<InvestmentReceivedDetails> CreateInvestmentReceivedDetailsAsync(InvestmentReceivedDetails receivedDetail)
        {
            try
            {
                await _appDbContext.InvestmentReceivedDetails.AddAsync(receivedDetail);
                await _appDbContext.SaveChangesAsync();
                _logger.LogInformation($"details added Successfully.");
                await _appDbContext.SaveChangesAsync();

                var clientid = _appDbContext.Investments.AsNoTracking()
                   .Where(i => i.InvestmentId == receivedDetail.InvestmentId).FirstOrDefaultAsync().GetAwaiter().GetResult().ClientId;

                var genfunc = new GenericFunctions(_appDbContext);
                await genfunc.AddNcAuditTrail(clientid,
                receivedDetail.InvestmentId,
                null,
                nameof(ConstantHelper.NCAuditModulesEnum.SPL_INVESTMENTS),
                $"Added Entry for Investment Received - Amount {receivedDetail.Amount} | Mode - {receivedDetail.Mode} | Banking Details - {receivedDetail.AccountNumberOrUpiId}",
                null,
                userClaims.UserLoginCode);

                return receivedDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while adding InvestmentReceivedDetails - {ex}");
                throw;
            }
        }

        public async Task<ClientBankingDetail> CreateClientBankingDetailAsync(ClientBankingDetail bankingDetail)
        {
            try
            {
                await _appDbContext.ClientBankingDetails.AddAsync(bankingDetail);
                await _appDbContext.SaveChangesAsync();
                _logger.LogInformation($"details added Successfully.");

                /*                var clientid = _appDbContext.Investments.AsNoTracking()
                                  .Where(i => i.InvestmentId == bankingDetail.InvestmentId).FirstOrDefaultAsync().GetAwaiter().GetResult().ClientId;*/

                var genfunc = new GenericFunctions(_appDbContext);
                await genfunc.AddNcAuditTrail(bankingDetail.ClientId,
                bankingDetail.InvestmentId,
                null,
                nameof(ConstantHelper.NCAuditModulesEnum.CLIENT_MASTER),
                $"Added Entry for Clients Banking Details - Mode {bankingDetail.Mode} | Accnt - {bankingDetail.AccountNoOrUpiId} | Bank Name - {bankingDetail.BankName}",
                null,
                userClaims.UserLoginCode);

                return bankingDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while adding ClientBankingDetail - {ex}");
                throw;
            }
        }

        public async Task<InvestmentReceivedDetails> DeleteInvestmentReceivedDetailsAsync(int id)
        {
            try
            {
                var receivedDetailEntity = await _appDbContext.InvestmentReceivedDetails.FirstOrDefaultAsync(i => i.Id == id);

                if (receivedDetailEntity == null)
                {
                    throw new Exception("InvestmentReceivedDetails not found.");
                }

                receivedDetailEntity.Status = false;
                await _appDbContext.SaveChangesAsync();
                _logger.LogInformation($"details deleted Successfully.");

                var clientid = _appDbContext.Investments.AsNoTracking()
                  .Where(i => i.InvestmentId == receivedDetailEntity.InvestmentId).FirstOrDefaultAsync().GetAwaiter().GetResult().ClientId;

                var genfunc = new GenericFunctions(_appDbContext);
                await genfunc.AddNcAuditTrail(clientid,
                receivedDetailEntity.InvestmentId,
                null,
                nameof(ConstantHelper.NCAuditModulesEnum.SPL_INVESTMENTS),
                $"Added Entry for Clients Banking Details - Mode {receivedDetailEntity.Mode} | Accnt - {receivedDetailEntity.AccountNumberOrUpiId} | Bank Name - {receivedDetailEntity.BankName}",
                null,
                userClaims.UserLoginCode);

                return receivedDetailEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting InvestmentReceivedDetails - {ex}");
                throw;
            }
        }

        public async Task<ClientBankingDetail> DeleteClientBankingDetailAsync(int id)
        {
            try
            {
                var bankingDetailEntity = await _appDbContext.ClientBankingDetails.FirstOrDefaultAsync(i => i.Id == id);
                if (bankingDetailEntity != null)
                {
                    bankingDetailEntity.Status = false;
                    await _appDbContext.SaveChangesAsync();
                    _logger.LogInformation($"details deleted Successfully.");
                    /*
                                        var clientid = _appDbContext.Investments.AsNoTracking()
                                     .Where(i => i.InvestmentId == bankingDetailEntity.InvestmentId).FirstOrDefaultAsync().GetAwaiter().GetResult().ClientId;*/

                    var genfunc = new GenericFunctions(_appDbContext);
                    await genfunc.AddNcAuditTrail(bankingDetailEntity.ClientId,
                    bankingDetailEntity.InvestmentId,
                    null,
                    nameof(ConstantHelper.NCAuditModulesEnum.CLIENT_MASTER),
                    $"Deleted Entry for Clients Banking Details - Mode {bankingDetailEntity.Mode} | Accnt - {bankingDetailEntity.AccountNoOrUpiId} | Bank Name - {bankingDetailEntity.BankName}",
                    null,
                    userClaims.UserLoginCode);

                    return bankingDetailEntity;
                }
                else
                {
                    throw new Exception("ClientBankingDetail not found.");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting ClientBankingDetail - {ex}");
                throw;
            }
        }
        public async Task SaveInvestmentBorrowLetterDetailsAsync(InvestmentBorrowLetterRequestDto borrowLetterDetails)
        {
            try
            {
                // Find the investment record by InvestmentId
                var investment = await _appDbContext.Investments
                    .Where(i => i.InvestmentId == borrowLetterDetails.InvestmentId)
                    .FirstOrDefaultAsync();

                // Log if investment is not found
                if (investment == null)
                {
                    _logger.LogWarning("Investment with InvestmentId: {InvestmentId} not found.", borrowLetterDetails.InvestmentId);
                    throw new Exception($"Investment with ID {borrowLetterDetails.InvestmentId} not found.");
                }

                // Update borrow letter details and status
                investment.BorrowLetter = borrowLetterDetails.BorrowLetter;
                investment.BorrowLetterStatus = 1;  // Status set to 1

                // Save changes to the database
                await _appDbContext.SaveChangesAsync();

                // Log success
                _logger.LogInformation("Successfully saved borrow letter details for InvestmentId: {InvestmentId}", borrowLetterDetails.InvestmentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving borrow letter details for InvestmentId: {InvestmentId}", borrowLetterDetails.InvestmentId);
                throw;
            }
        }

        public async Task UpdateInvestmentStatus(UpdateSubscriptionStatusRequestDto request)
        {
            Investment investment = await _appDbContext.Investments.Where(s => s.Guid == request.Guid).FirstOrDefaultAsync() ?? throw new Exception($"Investment not found with Investment ID  - {request.StatusToUpdate}");

            if (request.StatusToUpdate == (int)SubscriptionStatusEnum.Deleted &&
                investment.IsPaymentScheduleAvailable)
            {
                throw new Exception("This Investment is not allowed to be deleted. The payment schedule is already created.");

            }

            if (!(request.StatusToUpdate == (int)SubscriptionStatusEnum.Deleted) &&
                !investment.IsPaymentScheduleAvailable)
            {

                throw new Exception("This operation is not allowed on the Investment. Only you can delete the Investment.");

            }

            if (investment.InvestmentEndDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now.Date
                && (request.StatusToUpdate == (int)SubscriptionStatusEnum.Matured_Closed || request.StatusToUpdate == (int)SubscriptionStatusEnum.Matured_Renewed))
            {
                throw new Exception("This Investments's maturity date is not passed. Subscription is not allowed to closed.");
            }

            List<SplPaymentSchedule> payments = _appDbContext.SplPaymentSchedules.Where(p => p.InvestmentId == investment.InvestmentId && (p.Status == 0 || p.Status == 1)).ToList();

            if ((request.StatusToUpdate == (int)SubscriptionStatusEnum.Matured_Closed || request.StatusToUpdate == (int)SubscriptionStatusEnum.Matured_Renewed) && payments.Count > 0)
            {
                throw new Exception($"This Investment has {payments.Count} pending/inprogress payments in schedule. Close operation can not be done on this Investment.");
            }

            string auditTrailMessage = "";
            switch (request.StatusToUpdate)
            {
                case (int)SubscriptionStatusEnum.Active:
                    break;
                case (int)SubscriptionStatusEnum.ForceClosed:
                    investment.Status = request.StatusToUpdate;
                    investment.ClosingDate = DateTime.Now;
                    investment.ClosedBy = request.ActionBy;
                    investment.ClosingComment = request.Comments;
                    payments.ForEach(p => { p.Status = 5; p.Notes = request.Comments; });
                    auditTrailMessage = "Investment force closed.";
                    break;
                case (int)SubscriptionStatusEnum.Matured_Closed:
                    investment.Status = request.StatusToUpdate;
                    investment.ClosingDate = DateTime.Now;
                    investment.ClosedBy = request.ActionBy;
                    investment.ClosingComment = request.Comments;
                    auditTrailMessage = "Investment Matured and closed.";
                    break;
                case (int)SubscriptionStatusEnum.Matured_Renewed:
                    investment.Status = request.StatusToUpdate;
                    investment.ClosingDate = DateTime.Now;
                    investment.ClosedBy = request.ActionBy;
                    investment.ClosingComment = request.Comments;
                    auditTrailMessage = "Investment matured and renewed.";
                    break;
                case (int)SubscriptionStatusEnum.Deleted:
                    _appDbContext.Investments.Remove(investment);
                    auditTrailMessage = "Investment Deleted.";
                    break;

            }


            await _appDbContext.SaveChangesAsync();

            var genfunc = new GenericFunctions(_appDbContext);
            await genfunc.AddNcAuditTrail(investment.ClientId,
                investment.InvestmentId,
                null,
                nameof(ConstantHelper.NCAuditModulesEnum.SPL_INVESTMENTS),
                auditTrailMessage,
                null,
                investment.ClosedBy);
        }

        public async Task<List<ClientMaster>> GetAllClientsWithActionableInvestmentsAsync()
        {
            // Fetch all clients from the database
            var clients = await _appDbContext.ClientMasters
                                  .AsNoTracking()
                                  .ToListAsync();

            foreach (var client in clients)
            {
                await PopulateClientInvestmentDetailsAsync(client);
            }

            return clients;
        }

        public async Task PopulateClientInvestmentDetailsAsync(ClientMaster client)
        {
            if (client == null || client.ClientId <= 0)
            {
                return; // Skip if client is null or has invalid ID
            }

            // Fetch the client's investments
            client.Investments = await _appDbContext.Investments
                                        .Where(i => i.ClientId == client.ClientId)
                                        .ToListAsync();

            // Optionally get Business Dev Team Details (if applicable)
            var bdDetails = await _bdtRepo.GetBusinessDevByClientId(client.ClientId);
            client.RefferedByName = bdDetails?.Name ?? string.Empty;

            // Fetch investment details (ReceivedDetails, BankingDetails) for each investment
            foreach (var investment in client.Investments)
            {
                investment.RefferedByName = bdDetails?.Name ?? string.Empty;

                // Fetch InvestmentReceivedDetails
                investment.LstInvestmentReceivedDetails = await _appDbContext.InvestmentReceivedDetails
                                                        .Where(ird => ird.InvestmentId == investment.InvestmentId && ird.Status == true)
                                                        .ToListAsync();

                // Fetch ClientBankingDetails
                investment.LstClientBankingDetails = await _appDbContext.ClientBankingDetails
                                                     .Where(cbd => cbd.ClientId == investment.ClientId && cbd.Status == true)
                                                     .ToListAsync();

                // Calculate the total amount received from InvestmentReceivedDetails
                decimal totalReceivedAmount = investment.LstInvestmentReceivedDetails.Sum(ird => ird.Amount);

                // Compare the total received amount with the invested amount
                investment.IsFullyPaid = totalReceivedAmount >= investment.InvestmentAmount;

                // Check payment schedules
                if (investment.IsPaymentScheduleAvailable)
                {
                    var schedules = await _appDbContext.SplPaymentSchedules.AsNoTracking()
                                            .Where(p => p.InvestmentId == investment.InvestmentId)
                                            .ToListAsync();
                    if (schedules.Count != 0)
                    {
                        bool allPaymentsDone = schedules.Where(p => p.Status.Value == 0).Count() <= 0;
                        investment.IsInvestmentMatured = allPaymentsDone &&
                                                         investment.InvestmentEndDate.ToDateTime(TimeOnly.MinValue) <= DateTime.Now;
                    }
                    else
                    {
                        investment.IsInvestmentMatured = false;
                    }
                }
                else
                {
                    investment.IsInvestmentMatured = false;
                }
            }
        }

        public async Task<InvestmentReceivedDetails> AddInvestmentRecAttachement(int id, IFormFile? file)
        {
            try
            {
                byte[] compressedAttachment = [];
                if (file != null && file.Length > 0)
                {

                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        compressedAttachment = memoryStream.ToArray();
                    }
                }

                var receivedDetailEntity = await _appDbContext.InvestmentReceivedDetails.FirstOrDefaultAsync(i => i.Id == id);

                if (receivedDetailEntity != null)
                {
                    var receivedDetail = new InvestmentReceivedDetailDto
                    {
                        Id=receivedDetailEntity.Id,
                        ProcessId=receivedDetailEntity.ProcessId,
                        InvestmentId = receivedDetailEntity.InvestmentId,
                        InvestmentGuid = receivedDetailEntity.InvestmentGuid,
                        Mode = receivedDetailEntity.Mode,
                        BankName = receivedDetailEntity.BankName,
                        AccountNumberOrUpiId = receivedDetailEntity.AccountNumberOrUpiId,
                        Amount = receivedDetailEntity.Amount,
                        Comments = receivedDetailEntity.Comments,
                        AddedBy = receivedDetailEntity.AddedBy,
                        ReceivedDate = receivedDetailEntity.ReceivedDate,
                        Status = true,
                        InvestmentAttachment = compressedAttachment,
                    };

                    await UpdateInvestmentReceivedDetailsAsync(receivedDetail);
                    return receivedDetailEntity;
                }
                else
                {
                    throw new Exception("Details Not Found !");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating details - {ex}");
                throw;
            }

        }
    }
}
