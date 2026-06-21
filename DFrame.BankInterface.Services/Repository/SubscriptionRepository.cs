using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Migrations;
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.Dashboard;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Generic;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;
using ExpenseTracker.Services.Utilities;
using ExpenseTracker.Services.Utilities.ConstantHelper;
using NPOI.POIFS.Properties;
using System.Text;
using static ExpenseTracker.Services.Utilities.ConstantHelper.ConstantHelper;

namespace ExpenseTracker.Services.Repository
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDBContext _appDbContext;
        private readonly ILogger<SubscriptionRepository> _logger;
        private readonly IFeatureManager _featureManager;
        private readonly CommunicationConfigOptions _communicationConfigOptions;
        private readonly IMessageService _messageService;
        public SubscriptionRepository(AppDBContext appDBContext,
            ILogger<SubscriptionRepository> logger,
            IFeatureManager featureManager,
            IOptions<CommunicationConfigOptions> options
            , IMessageService messageService)
        {
            _appDbContext = appDBContext;
            _logger = logger;
            _featureManager = featureManager;
            _communicationConfigOptions = options.Value;
            _messageService = messageService;
        }
        public Task DeleteSubscriptionAsync(int subscriptionId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TblSubscription>> GetAllSubscriptionsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ClientMaster>> GetAllClientsWithSubscriptionsAsync()
        {
            List<ClientMaster> clients = await _appDbContext.ClientMasters.AsNoTracking().Include(x => x.Subscriptions).ToListAsync();
            return clients;
        }



        public async Task<TblSubscription> GetSubscriptionByIdAsync(int subscritptionId)
        {
            TblSubscription? subscription = await _appDbContext.Subscriptions.AsNoTracking()
                                            .Where(c => c.SubscriptionId == subscritptionId)
                                            .FirstOrDefaultAsync();
            return subscription;
        }

        public async Task<ClientMaster?> GetClientWithSubByClientIdAsync(int clientId)
        {
            ClientMaster? client = await _appDbContext.ClientMasters.AsNoTracking()
                                            .Where(c => c.ClientId == clientId)
                                            .Include(x => x.Subscriptions)
                                            .FirstOrDefaultAsync();
            return client;

        }

        public async Task<TblSubscription> CreateSubscriptionAsync(TblSubscription subscription)
        {
            try
            {
                var res = await _appDbContext.Subscriptions.AddAsync(subscription);
                await _appDbContext.SaveChangesAsync();

                //send notification to ceo
                if (_featureManager.IsEnabledAsync("NewInvestmentReceived").GetAwaiter().GetResult())
                {
                    var client = await _appDbContext.ClientMasters.AsNoTracking().Where(c => c.ClientId == subscription.ClientId).FirstOrDefaultAsync();
                    StringBuilder sb = new();
                    sb.Append(_communicationConfigOptions.MessageTemplates.Where(m => m.MessageType == "NewInvestmentReceived").FirstOrDefault().Message);
                    sb.Replace("{clientName}", $"{client.Name}");
                    sb.Replace("{investmentAmount}", $"INR {subscription.InvestmentAmount}");
                    sb.Replace("{interest}", $"INR {subscription.TotalInterest}");
                    sb.Replace("{PlanName}", $"INR {subscription.PlanName}");

                    string receiverNumbers = _communicationConfigOptions.Contacts.Where(c => c.Type == "ApproverMobile").FirstOrDefault().ContactNo;
                    await _messageService.SendMessage(sb.ToString(), receiverNumbers);
                }

                var genfunc = new GenericFunctions(_appDbContext);
                await genfunc.AddNcAuditTrail(subscription.ClientId,
                    res.Entity.SubscriptionId,
                    null,
                    nameof(ConstantHelper.NCAuditModulesEnum.SUBSCRIPTION),
                    $"New Investment created for - {subscription.ClientId}, Plan - {subscription.PlanName}, Amount - INR{subscription.InvestmentAmount}",
                    null,
                    subscription.CreatedBy);

                return res.Entity;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while adding new subscription - {ex}");
                throw;
            }
        }

        public async Task<TblSubscription> UpdateSubscriptionAsync(TblSubscription subscription)
        {
            try
            {
                _appDbContext.Subscriptions.Update(subscription).Property(p => p.SubscriptionId).IsModified = false;
                _appDbContext.Subscriptions.Update(subscription).Property(p => p.ClientMasterClientId).IsModified = false;
                _appDbContext.Subscriptions.Update(subscription).Property(p => p.ClientId).IsModified = false;



                await _appDbContext.SaveChangesAsync();

                var genfunc = new GenericFunctions(_appDbContext);
                await genfunc.AddNcAuditTrail(subscription.ClientMasterClientId,
                    subscription.SubscriptionId,
                    null,
                    nameof(ConstantHelper.NCAuditModulesEnum.SUBSCRIPTION),
                    $"Investment Updated",
                    null,
                    subscription.LastUpdatedBy);

                return subscription;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while adding new subscription - {ex}");
                throw;
            }
        }
        public async Task<List<TblPaymentSchedule>> GetPaymentSchedulesBySubScriptionIdAsync(int subscriptionId)
        {
            List<TblPaymentSchedule> paymentSchedules = await _appDbContext.PaymentSchedules
           .AsNoTracking()
           .Where(ps => ps.SubscriptionId == subscriptionId)
           .Include(ps => ps.ClientMaster)
           .Include(ps => ps.Subscription)
           .ToListAsync();

            return paymentSchedules;
        }

        public async Task<List<TblPaymentSchedule>> CreatePaymentScheduleForSubscriptionAsync(List<TblPaymentSchedule> paymentSchedules)
        {
            try
            {
                (await _appDbContext.Subscriptions.FirstOrDefaultAsync(s => s.SubscriptionId == paymentSchedules[0].SubscriptionId && s.ClientId == paymentSchedules[0].ClientId)).IsPaymentScheduleAvailable = true;
                await _appDbContext.PaymentSchedules.AddRangeAsync(paymentSchedules);
                await _appDbContext.SaveChangesAsync();


                var genfunc = new GenericFunctions(_appDbContext);
                await genfunc.AddNcAuditTrail(paymentSchedules[0].ClientId,
                    paymentSchedules[0].SubscriptionId,
                    null,
                    nameof(ConstantHelper.NCAuditModulesEnum.PAYMENTS),
                    $"Payment Schedule created for {paymentSchedules[0].SubscriptionId}, No. of Installments - {paymentSchedules.Count}, Total Interest to Pay - {paymentSchedules.Sum(s => s.InterestRate)} %, Total Amount As Interest - INR{paymentSchedules.Sum(s => s.PayableAmount)} ",
                    null,
                    paymentSchedules[0].CreatedBy);

                return paymentSchedules;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while adding new payment schedules - {ex}");
                throw;
            }
        }

        public async Task<List<PaymentScheduleDto>> GetAllDuedPaymentsFromPayScheduleAsync()
        {
            // Get the current date
            DateTime currentDate = DateTime.Now.Date;

            // Retrieve all due payments with due date less than current date and status equals 0
            var duedPayments = await _appDbContext.PaymentSchedules.AsNoTracking()
                //.Include(ps => ps.ClientMaster)
                .Where(p => p.DueDate.Value.Date < currentDate && (p.Status == 0 || p.Status == 1 || p.Status == 2))
                .Select(ps => new PaymentScheduleDto
                {
                    ScheduleId = ps.ScheduleId,
                    Guid = ps.Guid,
                    ClientId = ps.ClientId,
                    DueDate = ps.DueDate,
                    PayableAmount = ps.PayableAmount,
                    AmountPaid = ps.AmountPaid,
                    PaymentDate = ps.PaymentDate,
                    PaymentUtr = ps.PaymentUtr,
                    PaymentProofAttachment = ps.PaymentProofAttachment,
                    Status = ps.Status,
                    Notes = ps.Notes,
                    CreatedDate = ps.CreatedDate,
                    CreatedBy = ps.CreatedBy,
                    InterestRate = ps.InterestRate,
                    InvestedAmount = ps.InvestedAmount,
                    Day = ps.Day,
                    ClientName = ps.ClientMaster.Name,
                    SubscriptionId = ps.Subscription.SubscriptionId,
                    PrefferedPaymentMode = ps.Subscription.PayoutMethod,
                    PrefferedUpiId = ps.Subscription.UpiId,
                    PrefferedPayoutBankName = ps.Subscription.PayoutBankName,
                    PrefferedPayoutBankAccountNo = ps.Subscription.PayoutBankAccountNo,
                    PrefferedPayoutBankIfscCode = ps.Subscription.PayoutBankIfscCode,
                    PrefferedPayoutBankAccountHolderName = ps.Subscription.PayoutBankAccountHolderName,
                }).OrderBy(p => p.DueDate)
                    .ToListAsync();

            return duedPayments;
        }
        public async Task<List<PaymentScheduleDto>> GetAllTodaysPaymentsFromPayScheduleAsync()
        {
            // Get the current date
            DateTime currentDate = DateTime.Now.Date;

            // Retrieve all todays payments with due date equal to current date and status equals 0
            var todaysPayments = await _appDbContext.PaymentSchedules.AsNoTracking()
                //.Include(ps => ps.ClientMaster)
                .Where(p => EF.Functions.DateDiffDay(currentDate, p.DueDate.Value.Date) == 0 && (p.Status == 0 || p.Status == 1 || p.Status == 2))
                .Select(ps => new PaymentScheduleDto
                {
                    ScheduleId = ps.ScheduleId,
                    Guid = ps.Guid,
                    ClientId = ps.ClientId,
                    DueDate = ps.DueDate,
                    PayableAmount = ps.PayableAmount,
                    AmountPaid = ps.AmountPaid,
                    PaymentDate = ps.PaymentDate,
                    PaymentUtr = ps.PaymentUtr,
                    PaymentMode = ps.PaymentMode,
                    PaidBy = ps.PaidBy,
                    PaymentProofAttachment = ps.PaymentProofAttachment,
                    Status = ps.Status,
                    Notes = ps.Notes,
                    CreatedDate = ps.CreatedDate,
                    CreatedBy = ps.CreatedBy,
                    InterestRate = ps.InterestRate,
                    InvestedAmount = ps.InvestedAmount,
                    Day = ps.Day,
                    ClientName = ps.ClientMaster.Name,
                    SubscriptionId = ps.Subscription.SubscriptionId,
                    PrefferedPaymentMode = ps.Subscription.PayoutMethod,
                    PrefferedUpiId = ps.Subscription.UpiId,
                    PrefferedPayoutBankName = ps.Subscription.PayoutBankName,
                    PrefferedPayoutBankAccountNo = ps.Subscription.PayoutBankAccountNo,
                    PrefferedPayoutBankIfscCode = ps.Subscription.PayoutBankIfscCode,
                    PrefferedPayoutBankAccountHolderName = ps.Subscription.PayoutBankAccountHolderName,
                }).OrderBy(p => p.DueDate)
                    .ToListAsync();

            return todaysPayments;
        }
        public async Task<List<PaymentScheduleDto>> GetAllFuturePaymentsFromPayScheduleAsync()
        {
            // Get the current date
            DateTime currentDate = DateTime.Now.Date;

            // Calculate the future date (current date + 5 days)
            DateTime futureDate = currentDate.AddDays(5).Date;

            // Retrieve all records within the date range from current date till 5 days in the future
            var futurePayments = await _appDbContext.PaymentSchedules.AsNoTracking()
                //.Include(ps => ps.ClientMaster)
                .Where(p => p.DueDate.Value.Date > currentDate && p.DueDate.Value.Date <= futureDate)
               .Select(ps => new PaymentScheduleDto
               {
                   ScheduleId = ps.ScheduleId,
                   Guid = ps.Guid,
                   ClientId = ps.ClientId,
                   DueDate = ps.DueDate,
                   PayableAmount = ps.PayableAmount,
                   AmountPaid = ps.AmountPaid,
                   PaymentDate = ps.PaymentDate,
                   PaymentUtr = ps.PaymentUtr,
                   PaymentMode = ps.PaymentMode,
                   PaidBy = ps.PaidBy,
                   PaymentProofAttachment = ps.PaymentProofAttachment,
                   Status = ps.Status,
                   Notes = ps.Notes,
                   CreatedDate = ps.CreatedDate,
                   CreatedBy = ps.CreatedBy,
                   InterestRate = ps.InterestRate,
                   InvestedAmount = ps.InvestedAmount,
                   Day = ps.Day,
                   ClientName = ps.ClientMaster.Name,
                   SubscriptionId = ps.Subscription.SubscriptionId,
                   PrefferedPaymentMode = ps.Subscription.PayoutMethod,
                   PrefferedUpiId = ps.Subscription.UpiId,
                   PrefferedPayoutBankName = ps.Subscription.PayoutBankName,
                   PrefferedPayoutBankAccountNo = ps.Subscription.PayoutBankAccountNo,
                   PrefferedPayoutBankIfscCode = ps.Subscription.PayoutBankIfscCode,
                   PrefferedPayoutBankAccountHolderName = ps.Subscription.PayoutBankAccountHolderName,
               }).OrderBy(p => p.DueDate)
                    .ToListAsync();

            return futurePayments;
        }
        public async Task<List<PaymentScheduleDto>> GetAllPaymentsToProcessAsync()
        {
            var paymentsToProcess = await _appDbContext.PaymentSchedules
                .AsNoTracking()
                .Where(ps => ps.Status == 1)
               .Select(ps => new PaymentScheduleDto
               {
                   ScheduleId = ps.ScheduleId,
                   Guid = ps.Guid,
                   ClientId = ps.ClientId,
                   DueDate = ps.DueDate,
                   PayableAmount = ps.PayableAmount,
                   AmountPaid = ps.AmountPaid,
                   PaymentDate = ps.PaymentDate,
                   PaymentUtr = ps.PaymentUtr,
                   PaymentMode = ps.PaymentMode,
                   PaidBy = ps.PaidBy,
                   PaymentProofAttachment = ps.PaymentProofAttachment,
                   Status = ps.Status,
                   Notes = ps.Notes,
                   CreatedDate = ps.CreatedDate,
                   CreatedBy = ps.CreatedBy,
                   InterestRate = ps.InterestRate,
                   InvestedAmount = ps.InvestedAmount,
                   Day = ps.Day,
                   ClientName = ps.ClientMaster.Name,
                   SubscriptionId = ps.Subscription.SubscriptionId,
                   PrefferedPaymentMode = ps.Subscription.PayoutMethod,
                   PrefferedUpiId = ps.Subscription.UpiId,
                   PrefferedPayoutBankName = ps.Subscription.PayoutBankName,
                   PrefferedPayoutBankAccountNo = ps.Subscription.PayoutBankAccountNo,
                   PrefferedPayoutBankIfscCode = ps.Subscription.PayoutBankIfscCode,
                   PrefferedPayoutBankAccountHolderName = ps.Subscription.PayoutBankAccountHolderName,
               }).OrderBy(p => p.DueDate)
                .ToListAsync();

            return paymentsToProcess;

        }

        public async Task<List<PaymentScheduleDto>> GetPaymentHistories(DateTime startDate, DateTime endDate)
        {
            var paymentsToProcess = await _appDbContext.PaymentSchedules
                    .AsNoTracking()
                    .Where(ps => ps.DueDate.Value.Date > startDate && ps.DueDate.Value.Date <= endDate)
                   .Select(ps => new PaymentScheduleDto
                   {
                       ScheduleId = ps.ScheduleId,
                       Guid = ps.Guid,
                       ClientId = ps.ClientId,
                       DueDate = ps.DueDate,
                       PayableAmount = ps.PayableAmount,
                       AmountPaid = ps.AmountPaid,
                       PaymentDate = ps.PaymentDate,
                       PaymentUtr = ps.PaymentUtr,
                       PaymentMode = ps.PaymentMode,
                       PaidBy = ps.PaidBy,
                       PaymentProofAttachment = ps.PaymentProofAttachment,
                       Status = ps.Status,
                       Notes = ps.Notes,
                       CreatedDate = ps.CreatedDate,
                       CreatedBy = ps.CreatedBy,
                       InterestRate = ps.InterestRate,
                       InvestedAmount = ps.InvestedAmount,
                       Day = ps.Day,
                       ClientName = ps.ClientMaster.Name,
                       SubscriptionId = ps.Subscription.SubscriptionId,
                       PrefferedPaymentMode = ps.Subscription.PayoutMethod,
                       PrefferedUpiId = ps.Subscription.UpiId,
                       PrefferedPayoutBankName = ps.Subscription.PayoutBankName,
                       PrefferedPayoutBankAccountNo = ps.Subscription.PayoutBankAccountNo,
                       PrefferedPayoutBankIfscCode = ps.Subscription.PayoutBankIfscCode,
                       PrefferedPayoutBankAccountHolderName = ps.Subscription.PayoutBankAccountHolderName,
                   }).OrderBy(p => p.DueDate)
                    .ToListAsync();

            return paymentsToProcess;
        }

        public async Task UpdateMakersPaymentStatus(List<Guid> payments, int statusToUpdate, string comments, string updatedBy)
        {
            var paySchedules = await _appDbContext.PaymentSchedules.Where(p => payments.Contains(p.Guid)).ToListAsync();

            paySchedules.ForEach(p => { p.Status = statusToUpdate; p.Notes = comments; });

            await _appDbContext.SaveChangesAsync();

            //create audit trail
            var genfunc = new GenericFunctions(_appDbContext);
            var auditTrails = new List<AuditTrail>();

            //add audit trail
            foreach (var pay in paySchedules)
            {
                await genfunc.AddNcAuditTrail(
                     pay.ClientId,
                     pay.SubscriptionId,
                     pay.ScheduleId,
                     nameof(NCAuditModulesEnum.PAYMENTS),
                     $"Payment Status updated to - {((PaymentStatusEnum)statusToUpdate).ToString()}",
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

        public async Task UpdateManualPaymentStatus(UpdateManualPaymentStatusDto payment)
        {
            var schedule = await _appDbContext.PaymentSchedules.Where(p => p.Guid == payment.Guid).FirstOrDefaultAsync();

            schedule.Status = payment.Status;
            schedule.Notes = string.IsNullOrWhiteSpace(payment.Comments) ? schedule.Notes : payment.Comments;
            schedule.PaymentUtr = string.IsNullOrWhiteSpace(payment.PaymentUtr) ? schedule.PaymentUtr : payment.PaymentUtr;
            schedule.PaymentDate = DateTime.Now;
            schedule.PaymentMode = "Manual";
            schedule.AmountPaid = schedule.PayableAmount;
            schedule.PaidBy = payment.PaidBy;

            await _appDbContext.SaveChangesAsync();

            //send notification to customer
            if (_featureManager.IsEnabledAsync("PaymentComplete").GetAwaiter().GetResult())
            {
                var client = await GetClientDetailByPaymentId(payment.Guid);
                string cName = client != null ? client.Name : "Investor";

                StringBuilder sb = new();
                sb.Append(_communicationConfigOptions.MessageTemplates.Where(m => m.MessageType == "PaymentComplete").FirstOrDefault().Message);
                sb.Replace("{nameOfInvestor}", $"{cName}");
                sb.Replace("{amount paid}", $"{schedule.PayableAmount}");

                string receiverNumbers = client.Mobile;
                await _messageService.SendMessage(sb.ToString(), receiverNumbers);
            }


            var genfunc = new GenericFunctions(_appDbContext);
            await genfunc.AddNcAuditTrail(schedule.ClientId,
                schedule.SubscriptionId,
                schedule.ScheduleId,
                nameof(ConstantHelper.NCAuditModulesEnum.PAYMENTS),
                $"Manual Payment complete.",
                null,
                schedule.PaidBy);

        }

        public async Task<List<SubsWithPaymentsDto>> GetAllClientsSubscriptionWithPayments(int clientId)
        {
            try
            {
                var result = await _appDbContext.Subscriptions
                        .Where(sub => sub.ClientId == clientId)
                        .Select(sub => new SubsWithPaymentsDto
                        {
                            SubscriptionId = sub.SubscriptionId,
                            SubscriptionType = sub.SubscriptionType,
                            OldSubscriptionId = sub.OldSubscriptionId,
                            ClientId = clientId,
                            DateOfInvestment = sub.DateOfInvestment,
                            PlanCode = sub.PlanCode,
                            PlanName = sub.PlanName,
                            InvestmentAmount = sub.InvestmentAmount,
                            PayoutFrequency = sub.PayoutFrequency,
                            TotalInterest = sub.TotalInterest,
                            PayoutFrequencyInterestRate = sub.PayoutFrequencyInterestRate,
                            MaturityDate = sub.MaturityDate,
                            ApprovedBy = sub.ApprovedBy,
                            //ApprovedDate = sub.ApprovedDate?? DateTime.MinValue,
                            BorrowLetterStatus = sub.BorrowLetterStatus,
                            PayoutMethod = sub.PayoutMethod,
                            PayoutBankName = sub.PayoutBankName,
                            PayoutBankAccountNo = sub.PayoutBankAccountNo,
                            PayoutBankIfscCode = sub.PayoutBankIfscCode,
                            PayoutBankAccountHolderName = sub.PayoutBankAccountHolderName,
                            UpiId = sub.UpiId,
                            Notes = sub.Notes,
                            Status = sub.Status,
                            CreatedDate = sub.CreatedDate,
                            CreatedBy = sub.CreatedBy,
                            Tenure = sub.Tenure,
                            isPaymentScheduleAvailable = sub.IsPaymentScheduleAvailable,
                            Guid = sub.Guid,
                            Payments = _appDbContext.PaymentSchedules
                                    .Where(ps => ps.SubscriptionId == sub.SubscriptionId)
                                    .Select(ps => new PaymentScheduleDto
                                    {
                                        ScheduleId = ps.ScheduleId,
                                        ClientId = ps.ClientId,
                                        SubscriptionId = ps.SubscriptionId,
                                        DueDate = ps.DueDate,
                                        PayableAmount = ps.PayableAmount,
                                        AmountPaid = ps.AmountPaid,
                                        PaymentDate = ps.PaymentDate,
                                        PaymentMode = ps.PaymentMode,
                                        PaymentUtr = ps.PaymentUtr,
                                        PaymentProofAttachment = ps.PaymentProofAttachment,
                                        Status = ps.Status,
                                        Notes = ps.Notes,
                                        CreatedDate = ps.CreatedDate,
                                        CreatedBy = ps.CreatedBy,
                                        InterestRate = ps.InterestRate,
                                        InvestedAmount = ps.InvestedAmount,
                                        Day = ps.Day,
                                        Guid = ps.Guid,
                                        PaidBy = ps.PaidBy
                                    })
                                    .ToList(),
                        })
                        .ToListAsync();

                return result;

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task UpdateSubscriptionStatus(UpdateSubscriptionStatusRequestDto request)
        {

            TblSubscription subscription = await _appDbContext.Subscriptions.Where(s => s.Guid == request.Guid).FirstOrDefaultAsync() ?? throw new Exception($"Subscription not found with subscription ID  - {request.StatusToUpdate}");

            if (request.StatusToUpdate == (int)SubscriptionStatusEnum.Deleted &&
                subscription.IsPaymentScheduleAvailable)
            {
                throw new Exception("This subscription is not allowed to be deleted. The payment schedule is already created.");

            }

            if (!(request.StatusToUpdate == (int)SubscriptionStatusEnum.Deleted) &&
                !subscription.IsPaymentScheduleAvailable)
            {

                throw new Exception("This operation is not allowed on the Investment. Only you can delete the Investment.");

            }

            if (subscription.MaturityDate.Date >= DateTime.Now.Date
                && (request.StatusToUpdate == (int)SubscriptionStatusEnum.Matured_Closed || request.StatusToUpdate == (int)SubscriptionStatusEnum.Matured_Renewed))
            {
                throw new Exception("This Investments's maturity date is not passed. Subscription is not allowed to closed.");
            }

            List<TblPaymentSchedule> payments = _appDbContext.PaymentSchedules.Where(p => p.SubscriptionId == subscription.SubscriptionId && (p.Status == 0 || p.Status == 1)).ToList();

            if ((request.StatusToUpdate == (int)SubscriptionStatusEnum.Matured_Closed || request.StatusToUpdate == (int)SubscriptionStatusEnum.Matured_Renewed) && payments.Count > 0)
            {
                throw new Exception($"This Investment has {payments.Count} pending/inprogress payments in schedule. Close operation can not be done on this subscription.");
            }

            string auditTrailMessage = "";
            switch (request.StatusToUpdate)
            {
                case (int)SubscriptionStatusEnum.Active:
                    break;
                case (int)SubscriptionStatusEnum.ForceClosed:
                    subscription.Status = request.StatusToUpdate;
                    subscription.ClosingDate = DateTime.Now;
                    subscription.ClosedBy = request.ActionBy;
                    payments.ForEach(p => { p.Status = 5; p.Notes = request.Comments; });
                    auditTrailMessage = "Investment force closed.";
                    break;
                case (int)SubscriptionStatusEnum.Matured_Closed:
                    subscription.Status = request.StatusToUpdate;
                    subscription.ClosingDate = DateTime.Now;
                    subscription.ClosedBy = request.ActionBy;
                    auditTrailMessage = "Investment Matured and closed.";
                    break;
                case (int)SubscriptionStatusEnum.Matured_Renewed:
                    subscription.Status = request.StatusToUpdate;
                    subscription.ClosingDate = DateTime.Now;
                    subscription.ClosedBy = request.ActionBy;
                    subscription.OldSubscriptionId = request.OldSubscriptionId;
                    auditTrailMessage = "Investment matured and renewed.";
                    break;
                case (int)SubscriptionStatusEnum.Deleted:
                    _appDbContext.Subscriptions.Remove(subscription);
                    auditTrailMessage = "Investment Deleted.";
                    break;

            }

            subscription.Notes = request.Comments;

            await _appDbContext.SaveChangesAsync();

            var genfunc = new GenericFunctions(_appDbContext);
            await genfunc.AddNcAuditTrail(subscription.ClientId,
                subscription.SubscriptionId,
                null,
                nameof(ConstantHelper.NCAuditModulesEnum.SUBSCRIPTION),
                auditTrailMessage,
                null,
                subscription.ClosedBy);


        }

        public async Task<BorrowLetterDetailsResponseDto> GetBorrowLetterDetails(Guid subsGuid)
        {
            var result = await _appDbContext.BorrowLetterDetails.AsNoTracking().Where(b => b.SubscriptionGuid == subsGuid).FirstOrDefaultAsync();
            if (result != null)
            {
                BorrowLetterDetailsResponseDto responseDto = new BorrowLetterDetailsResponseDto()
                {
                    ChequeDate = result.ChequeDate,
                    ChequeNo = result.ChequeNo,
                    Guid = subsGuid,
                    SentDate = result.SentDate,
                    TrackingNo = result.TrackingNo
                };


                int status = _appDbContext.Subscriptions.AsNoTracking().Where(s => s.Guid == subsGuid).FirstOrDefaultAsync().GetAwaiter().GetResult().BorrowLetterStatus;
                responseDto.Status = status;
                return responseDto;
            }
            else
            {
                return null;
            }

        }

        public async Task AddBorrowLetterDetails(TblBorrowLetterDetails borrowLetterDetails, int status)
        {
            await _appDbContext.BorrowLetterDetails.AddAsync(borrowLetterDetails);
            var sub = await _appDbContext.Subscriptions.Where(s => s.Guid == borrowLetterDetails.SubscriptionGuid).FirstOrDefaultAsync();
            if (sub != null)
            {
                sub.BorrowLetterStatus = status;
            }
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateBorrowLetterDetails(TblBorrowLetterDetails borrowLetterDetails, int status)
        {
            var result = await _appDbContext.BorrowLetterDetails.Where(b => b.SubscriptionGuid == borrowLetterDetails.SubscriptionGuid).FirstOrDefaultAsync();
            result.ChequeNo = borrowLetterDetails.ChequeNo;
            result.ChequeDate = borrowLetterDetails.ChequeDate;
            result.SentDate = borrowLetterDetails.SentDate;
            result.TrackingNo = borrowLetterDetails.TrackingNo;

            var sub = await _appDbContext.Subscriptions.Where(s => s.Guid == borrowLetterDetails.SubscriptionGuid).FirstOrDefaultAsync();
            if (sub != null)
            {
                sub.BorrowLetterStatus = status;
            }
            await _appDbContext.SaveChangesAsync();

        }

        public BorrowLetterPrintDto GetBorrowLetterDetailsToPrint(Guid subsGuid)
        {
            SqlAccess sqlAccess = new SqlAccess(_appDbContext);

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@GUID", subsGuid.ToString());

            var result = sqlAccess.ExecuteScalarSp("USP_GET_DETAILS_FOR_BORROW_LETTER", param);

            var details = DatatableHelper.BindList<BorrowLetterPrintDto>(result);

            return details.Count > 0 ? details.FirstOrDefault() : new BorrowLetterPrintDto();
        }


        public async Task<ClientMaster> GetClientDetailByPaymentId(Guid guid)
        {
            var clientId = _appDbContext.PaymentSchedules.AsNoTracking().Where(p => p.Guid == guid).FirstOrDefaultAsync().GetAwaiter().GetResult().ClientId;
            var clientDetails = await _appDbContext.ClientMasters.Where(c => c.ClientId == clientId).FirstOrDefaultAsync();
            return clientDetails;
        }
    }
}
