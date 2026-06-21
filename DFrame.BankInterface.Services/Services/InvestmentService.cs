using AutoMapper;
using Microsoft.VisualBasic;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;
using ExpenseTracker.Services.Utilities;
using System.Security.Claims;
using static ExpenseTracker.Services.Utilities.ConstantHelper.ConstantHelper;

namespace ExpenseTracker.Services.Services
{
    public class InvestmentService : IInvestmentService
    {
        private readonly IInvestmentRepository _investmentRepository;
        private readonly IMapper _mapper;
        private ResponseDto _responseDto;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<InvestmentService> _logger;
        private readonly IBusinessDevTeamRepository _businessDevTeamRepository;

        public InvestmentService(IInvestmentRepository investmentRepository,
            IMapper mapper,
            ResponseDto responseDto,
            IHttpContextAccessor contextAccessor,
            ILogger<InvestmentService> logger
            ,IBusinessDevTeamService buinessDevTeamService
            ,IBusinessDevTeamRepository businessDevTeamRepository)
        {
            _investmentRepository = investmentRepository;
            _mapper = mapper;
            _responseDto = responseDto;
            _contextAccessor = contextAccessor;
            _logger = logger;
            _businessDevTeamRepository = businessDevTeamRepository;
        }

        public async Task<ResponseDto> CreateInvestmentAsync(InvestmentsDto investmentsDto)
        {
            try
            {
                var investment = await _investmentRepository.CreateInvestmentAsync(investmentsDto);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Investment created successfully";
                _responseDto.Result = investment;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while creating investment.");
            }
            return _responseDto;
        }

        public async Task<ResponseDto> UpdateInvestmentAsync(UpdateInvestmentsDto investmentsDto)
        {
            try
            {
                var investment = await _investmentRepository.UpdateInvestmentAsync(investmentsDto);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Investment updated successfully";
                _responseDto.Result = investment;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while updating investment.");
            }
            return _responseDto;
        }

        public async Task<ResponseDto> UpdateInvestmentReceivedDetailsAsync(InvestmentReceivedDetailDto receivedDetailDto)
        {
            try
            {
                var receivedDetail = await _investmentRepository.UpdateInvestmentReceivedDetailsAsync(receivedDetailDto);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Investment received details updated successfully";
                _responseDto.Result = receivedDetail;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while updating investment received details.");
            }
            return _responseDto;
        }
        public async Task<ResponseDto> UpdateClientBankingDetailAsync(ClientBankingDetailsDto bankingDetailDto)
        {
            try
            {
                var bankingDetail = await _investmentRepository.UpdateClientBankingDetailAsync(bankingDetailDto);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Client banking detail updated successfully";
                _responseDto.Result = bankingDetail;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while updating client banking detail.");
            }
            return _responseDto;
        }

        public async Task DeleteInvestment(int investmentId)
        {
            try
            {
                 await _investmentRepository.DeleteInvestment(investmentId);
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while deleting investment with Id: {investmentId}", investmentId);
            }
        }

        public async Task<ResponseDto> GetClientWithInvestmentByClientIdAsync(int clientId)
        {
            try
            {
                ClientMaster? client = await _investmentRepository.GetClientWithInvestmentByClientIdAsync(clientId);
                _responseDto.Result = client;
                _responseDto.IsSuccess = client != null;
                _responseDto.Message = client == null ? $"No record found for the client - {clientId}" : "";

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching client with investment by client Id: {ClientId}", clientId);
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetInvestmentByIdAsync(int investmentId)
        {
            try
            {
                Investment? investment = await _investmentRepository.GetInvestmentByIdAsync(investmentId);
                _responseDto.Result = investment;
                _responseDto.IsSuccess = investment != null;
                _responseDto.Message = investment == null ? $"No record found for the investment - {investmentId}" : "";

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching investment with Id: {investmentId}", investmentId);
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetAllInvestments()
        {
            try
            {
                List<Investment> inv = await _investmentRepository.GetAllInvestments();
                var investments = _mapper.Map<List<InvestmentsDto>>(inv);
                _responseDto.Result = investments;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching investments.");
            }
            return _responseDto;

        }

        public async Task<ResponseDto> GenerateSplPaymentSchedule(int inestmentid)
        {
            var invDetails = await _investmentRepository.GetInvestmentByIdAsync(inestmentid);

            var paySchedule = await GeneratePaymentSchedules(invDetails);

            _responseDto.Result = paySchedule;
            return _responseDto;
        }

        private async Task<List<SplPaymentScheduleDto>> GeneratePaymentSchedules(Investment investment)
        {
            if (investment == null)
                throw new ArgumentNullException(nameof(investment), "Investment data cannot be null.");

            if (investment.InvestmentAmount <= 0 || investment.PayoutFrequency <= 0 || investment.InvestmentTenure <= 0)
                throw new ArgumentException("Invalid investment details provided.");

            var schedules = new List<SplPaymentScheduleDto>();

            //0. joining bonus
            if (investment.IsJoiningBonusApplicable) {
                var joiningBonusSchedule = GenerateJoiningBonusSchedule(investment);
                schedules.Add(joiningBonusSchedule);
            }

            // 1. Client's Profit Payments Schedule
            var profitSchedules = GenerateProfitPaymentSchedules(investment);
            schedules.AddRange(profitSchedules);

            // 2. Referral Bonus First Payment Schedule
            string referralName = string.Empty;
            int referralId = 0;
            if (investment.IsReferralBonusApplicable)
            {
                var bdDetails = await _businessDevTeamRepository.GetBusinessDevByClientId(investment.ClientId);
                if (bdDetails != null) {
                    referralName = bdDetails.Name;
                    referralId = bdDetails.ClientId;
                }
            }
            if (investment.IsReferralBonusApplicable && investment.ReferralFirstBonusPercent > 0)
            {
                DateOnly? firstDueDate;

                // Determine the appropriate schedule based on whether the Joining Bonus is applicable
                var relevantSchedules = investment.IsJoiningBonusApplicable
                    ? schedules.Where(x => x.PaymentType == 0) // Joining bonus case
                    : profitSchedules; // Profit schedules if no joining bonus

                // Get the first due date
                var firstDueSchedule = relevantSchedules.OrderBy(s => s.DueDate).FirstOrDefault();
                firstDueDate = firstDueSchedule?.DueDate;

                var referralFirstBonusSchedule = GenerateReferralBonusSchedule(investment, investment.ReferralFirstBonusPercent, (int)SplPaymentTypesEnum.ReferralBonus,referralName,referralId, firstDueDate);
                schedules.Add(referralFirstBonusSchedule);
            }

            // 4. Capital Return Schedule
            var capitalReturnSchedule = GenerateCapitalReturnSchedule(investment);
            schedules.Add(capitalReturnSchedule);

            // 3. Maturity Bonus Schedule
            if (investment.IsMaturityBonusApplicable)
            {
                var maturityBonusSchedule = GenerateMaturityBonusSchedule(investment);
                schedules.Add(maturityBonusSchedule);
            }
          
            // 5. Referral Last Bonus Schedule
            if (investment.IsReferralBonusApplicable && investment.ReferralLastBonusPercent > 0)
            {
                var referralLastBonusSchedule = GenerateReferralBonusSchedule(investment, investment.ReferralLastBonusPercent, (int)SplPaymentTypesEnum.ReferralBonus, referralName,referralId,capitalReturnSchedule.DueDate);
                schedules.Add(referralLastBonusSchedule);
            }

            return schedules.OrderBy(s => s.DueDate).ToList();
        }

        private List<SplPaymentScheduleDto> GenerateProfitPaymentSchedules(Investment investment)
        {
            var schedules = new List<SplPaymentScheduleDto>();

            // Validating essential data
            if (investment.PayoutFrequencyProfitRatePercent <= 0)
                throw new ArgumentException("Invalid PayoutFrequencyProfitRatePercent.");

            var n = investment.InvestmentTenure / investment.PayoutFrequency;
            DateOnly currentPaymentDate;

            currentPaymentDate = n == investment.InvestmentTenure
                ? investment.InvestmentStartDate.AddDays(3)
                : investment.InvestmentStartDate.AddMonths(n);

            for (int i = 1; i <= n; i++)
            {
                // Move to next payment date
                currentPaymentDate = investment.InvestmentStartDate.AddMonths(i * investment.PayoutFrequency);
                currentPaymentDate = AdjustForWeekend(currentPaymentDate);
               

                var profitAmount = CalculateProfitAmount(investment.InvestmentAmount, investment.PayoutFrequencyProfitRatePercent);
                var tds = CalculateTds(profitAmount, investment.TdsPercent, investment.IsTdsApplicable);
                var payableAmount = profitAmount - tds;

                schedules.Add(CreatePaymentScheduleDto(investment, 
                    (int)SplPaymentTypesEnum.ClientProfit, 
                    currentPaymentDate, 
                    profitAmount, 
                    tds, 
                    payableAmount, 
                    currentPaymentDate.DayOfWeek.ToString()
                    ,investment.PayoutFrequencyProfitRatePercent
                    ,investment.TdsPercent));

                
            }

            return schedules;
        }

        private SplPaymentScheduleDto GenerateReferralBonusSchedule(Investment investment, decimal referralBonusPercent, int paymentType, string refferedBy,int referredById,DateOnly? referralDueDate = null)
        {
            //var dueDate = AdjustForWeekend(referralDueDate ?? investment.InvestmentStartDate.AddDays(3));
            var dueDate = AdjustForWeekend(referralDueDate == null ? investment.InvestmentStartDate.AddDays(3) : referralDueDate.Value);

            var profitAmount = CalculateProfitAmount(investment.InvestmentAmount, referralBonusPercent);
            var tds = CalculateTds(profitAmount, investment.TdsPercent, investment.IsTdsApplicable);
            var payableAmount = profitAmount - tds;

            return CreatePaymentScheduleDto(investment, paymentType, dueDate, profitAmount, tds, payableAmount, dueDate.DayOfWeek.ToString(),referralBonusPercent, investment.TdsPercent,refferedBy,referredById);
        }


        private SplPaymentScheduleDto GenerateJoiningBonusSchedule(Investment investment)
        {
            DateOnly dueDate = AdjustForWeekend(investment.InvestmentStartDate.AddDays(3));

            var profitAmount = CalculateProfitAmount(investment.InvestmentAmount, investment.JoiningBonusPercent);
            var tds = CalculateTds(profitAmount, investment.TdsPercent, investment.IsTdsApplicable);
            var payableAmount = profitAmount - tds;
            return CreatePaymentScheduleDto(investment, (int)SplPaymentTypesEnum.JoiningBonus, dueDate, profitAmount, tds, payableAmount, dueDate.DayOfWeek.ToString(), investment.JoiningBonusPercent, investment.TdsPercent);
        }


        private SplPaymentScheduleDto GenerateMaturityBonusSchedule(Investment investment)
        {
            DateOnly dueDate = investment.BonusTime == "start"
                ? AdjustForWeekend(investment.InvestmentStartDate.AddDays(3))
                : AdjustForWeekend(investment.InvestmentEndDate.AddDays(investment.CapitalLockingReturnPeriod + investment.CapitalReturnPeriod + 3));

            var profitAmount = CalculateProfitAmount(investment.InvestmentAmount, investment.BonusPercent);
            var tds = CalculateTds(profitAmount, investment.TdsPercent, investment.IsTdsApplicable);
            var payableAmount = profitAmount - tds;

            return CreatePaymentScheduleDto(investment, (int)SplPaymentTypesEnum.MaturityBonus, dueDate, profitAmount, tds, payableAmount, dueDate.DayOfWeek.ToString(), investment.BonusPercent, investment.TdsPercent);
        }

        private SplPaymentScheduleDto GenerateCapitalReturnSchedule(Investment investment)
        {
            var dueDate = AdjustForWeekend(investment.InvestmentEndDate.AddDays(investment.CapitalLockingReturnPeriod + investment.CapitalReturnPeriod));
            var profitAmount = investment.InvestmentAmount; // Return capital amount, no TDS
            var payableAmount = profitAmount;

            return CreatePaymentScheduleDto(investment, (int)SplPaymentTypesEnum.CapitalReturn, dueDate, profitAmount, 0, payableAmount,dueDate.DayOfWeek.ToString(),100, 0);
        }

        private decimal CalculateProfitAmount(decimal investmentAmount, decimal percent)
        {
            return investmentAmount * percent / 100;
        }

        private decimal CalculateTds(decimal profitAmount, decimal tdsPercent, bool isTdsApplicable)
        {
            return isTdsApplicable ? profitAmount * tdsPercent / 100 : 0;
        }

        private SplPaymentScheduleDto CreatePaymentScheduleDto(Investment investment,
            int paymentType,
            DateOnly dueDate,
            decimal profitAmount,
            decimal tds,
            decimal payableAmount,
            string day,
            decimal interestRate,
            decimal tdsPercent,
            string? receiversName = null,
            int? receiversId = null
            )
        {
            return new SplPaymentScheduleDto
            {
                Guid = Guid.NewGuid(),
                ClientId = receiversId ?? investment.ClientId,
                InvestmentId = investment.InvestmentId,
                PaymentType = paymentType,
                DueDate = dueDate,
                ProfitAmount = profitAmount,
                Tds = tds,
                PayableAmount = payableAmount,
                Status = 0 ,// Pending
                ClientName= receiversName ?? investment.ClientName,
                Day= day,
                InterestRate= interestRate,
                InvestedAmount = investment.InvestmentAmount,
                TdsPercent=tdsPercent,


            };
        }

        private DateOnly AdjustForWeekend(DateOnly date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Saturday => date.AddDays(2),
                DayOfWeek.Sunday => date.AddDays(1),
                _ => date
            };
        }

        public async Task<ResponseDto> CreateInvestmentPaymentSchedulesAsync(List<SplPaymentScheduleDto> schedulesDto)
        {
            try
            {
                _logger.LogInformation("Fetching ClientMaster data for client names.");

                // Get the list of ClientIds from the DTOs
                var clientIds = schedulesDto.Select(dto => dto.ClientId).Distinct().ToList();

                // Fetch ClientMaster data using repository method
                var clientMasterData = await _investmentRepository.GetClientNamesByIdsAsync(clientIds);


                _logger.LogInformation("Mapping DTO to entity for payment schedule creation.");

                // Map DTO to Entity and join with ClientMaster to fetch ClientName
                var paymentSchedules = schedulesDto.Select(dto => new SplPaymentSchedule
                {
                    Guid = Guid.NewGuid(),
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
                }).ToList();

                _logger.LogInformation("Calling repository to save payment schedules.");

                // Save to DB and get saved entities with their new primary keys
                var savedSchedules = await _investmentRepository.SaveInvestmentPaymentSchedulesAsync(paymentSchedules);

                _responseDto.IsSuccess = true;
                _responseDto.Message = "Payment schedules created successfully.";
                _responseDto.Result = savedSchedules;  // Return the saved entities with their primary keys
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the service while creating payment schedules.");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        public async Task<ResponseDto> GetSplPaymentScheduleByInvId(int inestmentid)
        {
            try
            {
               var schedules = await _investmentRepository.GetSplPaymentScheduleByInvestmentId(inestmentid);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "success";
                _responseDto.Result = schedules;  // Return the saved entities with their primary keys
            }
            catch (Exception ex) 
            {

                _logger.LogError(ex, "An error occurred while fetching the schedules.");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        public async Task<ResponseDto> GetAllTodaysPaymentsFromInvestPayScheduleAsync()
        {
            try
            {
                List<SplPaymentScheduleDto> paymentSchedules = await _investmentRepository.GetAllTodaysPaymentsFromInvestPayScheduleAsync();
                _responseDto.Result = paymentSchedules;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching today's payments.");
            }
            return _responseDto;
        }
        public async Task<ResponseDto> GetAllDuedSplPaymentsFromInvestmentPayScheduleAsync()
        {
            try
            {
                List<SplPaymentScheduleDto> paymentSchedules = await _investmentRepository.GetAllDuedSplPaymentsFromInvestmentPayScheduleAsync();
                _responseDto.Result = paymentSchedules;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching investment due payments.");
            }
            return _responseDto;
        }
        public async Task<ResponseDto> GetInvestmentPaymentHistoriesAsync(DateOnly startDate, DateOnly endDate)
        {
            try
            {
                var paymentHistories = await _investmentRepository.GetInvestmentPaymentHistoriesAsync(startDate, endDate);

                _responseDto.Result = paymentHistories;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching payment histories.");
            }

            return _responseDto;
        }
        public async Task<ResponseDto> GetAllInvestmentPaymentsToProcessAsync()
        {
            try
            {
                var allPayments = await _investmentRepository.GetAllInvestmentPaymentsToProcessAsync();

                _responseDto.Result = allPayments;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching all investment payments to process.");
            }

            return _responseDto;
        }
        public async Task<ResponseDto> GetAllExternalPaymentsAsync()
        {
            try
            {
                var payments = await _investmentRepository.GetAllExternalPaymentsAsync();
                _responseDto.Result = payments.Select(p => new ExternalPaymentsDto
                {
                    Id = p.Id,
                    TransferredFrom = p.TransferredFrom,
                    TransferredTo = p.TransferredTo,
                    Amount = p.Amount,
                    DrCr = p.DrCr,
                    TransactionDate = p.TransactionDate,
                    Description = p.Description,
                    Status = p.Status,
                    CreatedDate = p.CreatedDate,
                    CreatedBy = p.CreatedBy
                }).ToList();
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all external payments.");
                _responseDto.IsSuccess = false;
                _responseDto.Message = "An error occurred while retrieving external payments.";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetExternalPaymentByIdAsync(int id)
        {
            try
            {
                var payment = await _investmentRepository.GetExternalPaymentsByIdAsync(id);
                if (payment == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "External Payment not found";
                    return _responseDto;
                }

                _responseDto.Result = new ExternalPaymentsDto
                {
                    Id = payment.Id,
                    TransferredFrom = payment.TransferredFrom,
                    TransferredTo = payment.TransferredTo,
                    Amount = payment.Amount,
                    DrCr = payment.DrCr,
                    TransactionDate = payment.TransactionDate,
                    Description = payment.Description,
                    Status = payment.Status,
                    CreatedDate = payment.CreatedDate,
                    CreatedBy = payment.CreatedBy
                };
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching external payment by ID: {Id}", id);
                _responseDto.IsSuccess = false;
                _responseDto.Message = "An error occurred while retrieving the external payment.";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> AddExternalPaymentAsync(ExternalPaymentsDto paymentDto)
        {
            try
            {
                var payment = new ExternalPayment
                {
                    TransferredFrom = paymentDto.TransferredFrom,
                    TransferredTo = paymentDto.TransferredTo,
                    Amount = paymentDto.Amount,
                    DrCr = paymentDto.DrCr,
                    TransactionDate = paymentDto.TransactionDate,
                    Description = paymentDto.Description,
                    Status = paymentDto.Status,
                    CreatedBy = paymentDto.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                var addedPayment = await _investmentRepository.AddExternalPaymentsAsync(payment);
                paymentDto.Id = addedPayment.Id;
                _responseDto.Result = paymentDto;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding external payment.");
                _responseDto.IsSuccess = false;
                _responseDto.Message = "An error occurred while adding the external payment.";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> UpdateExternalPaymentAsync(ExternalPaymentsDto paymentDto)
        {
            try
            {
                var payment = await _investmentRepository.GetExternalPaymentsByIdAsync(paymentDto.Id);
                if (payment == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Payment not found.";
                    return _responseDto;
                }

                payment.TransferredFrom = paymentDto.TransferredFrom;
                payment.TransferredTo = paymentDto.TransferredTo;
                payment.Amount = paymentDto.Amount;
                payment.DrCr = paymentDto.DrCr;
                payment.TransactionDate = paymentDto.TransactionDate;
                payment.Description = paymentDto.Description;
                payment.Status = paymentDto.Status;

                var updatedPayment = await _investmentRepository.UpdateExternalPaymentsAsync(payment);
                _responseDto.Result = paymentDto;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating external payment.");
                _responseDto.IsSuccess = false;
                _responseDto.Message = "An error occurred while updating the external payment.";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> DeleteExternalPaymentAsync(int id)
        {
            try
            {
                await _investmentRepository.DeleteExternalPaymentsAsync(id);
                _responseDto.Result = true;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting external payment.");
                _responseDto.IsSuccess = false;
                _responseDto.Message = "An error occurred while deleting the external payment.";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> UpdateMakerInvestmentPaymentScheduleStatusAsync(List<Guid> payments, int statusToUpdate, string comments)
        {
            try
            {
                var userDetails = UserClaimsHelper.GetClaims(_contextAccessor.HttpContext.User.Identity as ClaimsIdentity);
                await _investmentRepository.UpdateMakerInvestmentPaymentScheduleStatusAsync(payments, statusToUpdate, comments, userDetails.UserLoginCode);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while updating makers investment payment status.");
            }
            return _responseDto;
        }

        public async Task<ResponseDto> UpdateManualInvestmentPaymentScheduleStatusAsync(UpdateManualInvestmentPaymentStatusDto payment)
        {
            try
            {
                var userDetails = UserClaimsHelper.GetClaims(_contextAccessor.HttpContext.User.Identity as ClaimsIdentity);
                payment.PaidBy = userDetails.UserName;

                await _investmentRepository.UpdateManualInvestmentPaymentScheduleStatusAsync(payment);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while updating manual investment payment status.");
            }
            return _responseDto;
        }

        public async Task<ResponseDto> CreateInvestmentReceivedDetailsAsync(InvestmentReceivedDetailDto receivedDetailDto)
        {
            try
            {
                InvestmentReceivedDetails details = _mapper.Map<InvestmentReceivedDetails>(receivedDetailDto);
                var receivedDetail = await _investmentRepository.CreateInvestmentReceivedDetailsAsync(details);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Investment received details added successfully";
                _responseDto.Result = receivedDetail;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while adding investment received details.");
            }
            return _responseDto;
        }
        public async Task<ResponseDto> CreateClientBankingDetailAsync(ClientBankingDetailsDto bankingDetailDto)
        {
            try
            {
                ClientBankingDetail details = _mapper.Map<ClientBankingDetail>(bankingDetailDto);
                var bankingDetail = await _investmentRepository.CreateClientBankingDetailAsync(details);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Client banking detail added successfully";
                _responseDto.Result = bankingDetail;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while adding client banking detail.");
            }
            return _responseDto;
        }

        public async Task<ResponseDto> DeleteInvestmentReceivedDetailsAsync(int id)
        {
            try
            {
                var receivedDetail = await _investmentRepository.DeleteInvestmentReceivedDetailsAsync(id);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Investment received details deleted successfully";
                _responseDto.Result = receivedDetail;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while deleting investment received details.");
            }
            return _responseDto;
        }
        public async Task<ResponseDto> DeleteClientBankingDetailAsync(int id)
        {
            try
            {
                var bankingDetail = await _investmentRepository.DeleteClientBankingDetailAsync(id);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Client banking detail deleted successfully";
                _responseDto.Result = bankingDetail;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while deleting client banking detail.");
            }
            return _responseDto;
        }
        public async Task<ResponseDto> SaveInvestmentBorrowLetterDetailsAsync(InvestmentBorrowLetterRequestDto borrowwLetter)
        {
            try
            {
                await _investmentRepository.SaveInvestmentBorrowLetterDetailsAsync(borrowwLetter);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while saving investment borrow letter details.");
            }
            return _responseDto;

        }

        public async Task<ResponseDto> UpdateInvestmentStatus(UpdateSubscriptionStatusRequestDto request)
        {
            try
            {
                await _investmentRepository.UpdateInvestmentStatus(request);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while updating Investment status.");
            }

            return _responseDto;
        }
        public async Task<ResponseDto> GetAllClientsWithActionableInvestmentsAsync()
        {
            try
            {
                List<ClientMaster> clients = await _investmentRepository.GetAllClientsWithActionableInvestmentsAsync();

                List<ClientMaster> actionableClients = new();
                foreach (ClientMaster client in clients)
                {
                    int isActionable = 0;
                    //foreach (var sub in client.Investments)
                    //{
                    //    isActionable = ((sub.MaturityDate.Date <= DateTime.Now.Date && sub.Status == (int)SubscriptionStatusEnum.Active) || !sub.IsPaymentScheduleAvailable) ? isActionable + 1 : isActionable;
                    //}
                    //if (isActionable > 0)
                    //{
                    //    actionableClients.Add(client);
                    //}
                }
                var clientsWithInvestments = _mapper.Map<List<ClientWithInvestmentDto>>(actionableClients);
                _responseDto.Result = clientsWithInvestments;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching clients with actionable investments.");
            }
            return _responseDto;

        }

        public async Task<ResponseDto> AddInvestmentRecAttachement(int id, IFormFile? file)
        {
            try
            {
                await _investmentRepository.AddInvestmentRecAttachement(id, file);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Invesment Received Attachement added successfully";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while adding Investment attachement.");
            }

            return _responseDto;
        }
    }
}
