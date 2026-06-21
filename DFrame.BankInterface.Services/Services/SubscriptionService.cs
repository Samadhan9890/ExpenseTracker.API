using AutoMapper;
using ExpenseTracker.Services.Migrations;
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
	public class SubscriptionService : ISubscriptionService
	{
		private readonly ISubscriptionRepository _subscriptionRepo;
		private readonly IMapper _mapper;
		private ResponseDto _responseDto;
		private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<SubscriptionService> _logger;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository,
			IMapper mapper,
			ResponseDto responseDto,
			IHttpContextAccessor contextAccessor, ILogger<SubscriptionService> logger)
        {
            _subscriptionRepo = subscriptionRepository;
			_mapper = mapper;
			_responseDto = responseDto;
			_contextAccessor = contextAccessor;
            _logger = logger;
        }

		

		public async Task<ResponseDto> GetAllClientsWithSubscriptionsAsync()
		{
			try
			{
				List<ClientMaster> clients = await _subscriptionRepo.GetAllClientsWithSubscriptionsAsync();
				var clientsWithSubs = _mapper.Map<List<ClientWithSubscriptionDto>>(clients);
				_responseDto.Result = clientsWithSubs;
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching clients with subscriptions.");
            }
            return _responseDto;			
			
		}

		public async Task<ResponseDto> GetAllClientsWithActionableSubscriptionsAsync()
		{
			try
			{
				List<ClientMaster> clients = await _subscriptionRepo.GetAllClientsWithSubscriptionsAsync();

                List<ClientMaster> actionableClients = new();
                foreach (ClientMaster client in clients)
                {
                    int isActionable = 0;
                    foreach (var sub in client.Subscriptions)
                    {
                        isActionable = ((sub.MaturityDate.Date <= DateTime.Now.Date  && sub.Status == (int)SubscriptionStatusEnum.Active) || !sub.IsPaymentScheduleAvailable) ? isActionable+1 : isActionable;
                    }
                    if(isActionable > 0)
                    {
                        actionableClients.Add(client);
                    }
                }
				var clientsWithSubs = _mapper.Map<List<ClientWithSubscriptionDto>>(actionableClients);
				_responseDto.Result = clientsWithSubs;
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching clients with actionable subscriptions.");
            }
            return _responseDto;

		}

		public async Task<ResponseDto> GetClientWithSubsByClientIdAsync(int clientId)
		{
			try
			{
				ClientMaster? client = await _subscriptionRepo.GetClientWithSubByClientIdAsync(clientId);
                foreach (var sub in client?.Subscriptions)
                {
                    sub.IsActionable = (sub.MaturityDate.Date <= DateTime.Now.Date && sub.Status == (int)SubscriptionStatusEnum.Active) ? true  : false;
                }
                _responseDto.Result = client;
				_responseDto.IsSuccess = client != null;
				_responseDto.Message = client == null ? $"No record found for the client - {clientId}":"";

			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess=false;
				_responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching client with subscription by client Id: {ClientId}", clientId);
            }
            return _responseDto;
		}

		public async Task<ResponseDto> CreateSubscriptionAsync(AddSubscriptionDto addSubscriptionDto)
		{
			try
			{
                //this function returns 3 values
                //1. bool if maturity date is correct
                //2. difference in months
                //3. total installments or payouts
                var maturirtyInfo = CheckValidMaturity(addSubscriptionDto);
				if (!maturirtyInfo.Item1)
				{
					throw new Exception("Please select correct maturity date. It should be in series of payout frequency. Also please make sure to have correct DoI and DoM date.");
				}
				
				var userDetails = UserClaimsHelper.GetClaims(_contextAccessor.HttpContext.User.Identity as ClaimsIdentity);

				TblSubscription subscriptionToAdd = _mapper.Map<TblSubscription>(addSubscriptionDto);


                subscriptionToAdd.TotalInterest = maturirtyInfo.Item3 * subscriptionToAdd.PayoutFrequencyInterestRate;
                subscriptionToAdd.Tenure = maturirtyInfo.Item2;
				subscriptionToAdd.CreatedBy = userDetails.UserName;
				subscriptionToAdd.IsPaymentScheduleAvailable = false;
				subscriptionToAdd.BorrowLetterStatus = 0;
				subscriptionToAdd.OldSubscriptionId = subscriptionToAdd.OldSubscriptionId == 0 ? null : subscriptionToAdd.OldSubscriptionId;
                subscriptionToAdd.ClientMasterClientId = addSubscriptionDto.ClientId;                
				_responseDto.Result = await _subscriptionRepo.CreateSubscriptionAsync(subscriptionToAdd);				
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = $"Unable to create subscription. - {ex.Message}";
                _logger.LogError(ex, "An error occurred while creating subscription.");
            }
            return _responseDto;
		}

		private Tuple<bool,int,int> CheckValidMaturity(AddSubscriptionDto subscriptionDto)
		{
			var res = ((subscriptionDto.MaturityDate.Year - subscriptionDto.DateOfInvestment.Year) * 12) + subscriptionDto.MaturityDate.Month - subscriptionDto.DateOfInvestment.Month;
			if (res % subscriptionDto.PayoutFrequency != 0)
			{
				return Tuple.Create(false, res,0);
			}
            if(subscriptionDto.DateOfInvestment.Day != subscriptionDto.MaturityDate.Day)
            {
				return Tuple.Create(false, res, 0);

			}
			return Tuple.Create(true, res,res/subscriptionDto.PayoutFrequency);
		}
        public async Task<ResponseDto> GetSubscriptionByIdAsync(int subscriptionId)
        {
            try
            {
                TblSubscription? subscription = await _subscriptionRepo.GetSubscriptionByIdAsync(subscriptionId);
                _responseDto.Result = subscription;
                _responseDto.IsSuccess = subscription != null;
                _responseDto.Message = subscription == null ? $"No record found for the Subscription - {subscriptionId}" : "";

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching subscription with Id: {SubscriptionId}", subscriptionId);
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GeneratePaymentScheduleBySubScriptionIdAsync(int subscriptionId)
        {
            List<PaymentScheduleDto> paymentSchedules = new List<PaymentScheduleDto>();
            try
            {
                _logger.LogInformation("Generating payment schedule for subscription Id: {SubscriptionId}", subscriptionId);
                TblSubscription? subscription = await _subscriptionRepo.GetSubscriptionByIdAsync(subscriptionId);
                if (subscription != null)
                {
                    paymentSchedules = GenerateSchedules(subscription);
                    _responseDto.Result = paymentSchedules;
                    _responseDto.IsSuccess = paymentSchedules != null;
                    _responseDto.Message = (paymentSchedules == null || paymentSchedules.Count == 0) ? $"No payout schedules found for the subscription with Id - {subscriptionId}" : "";
                    _logger.LogInformation("Payment schedule generated for subscription Id: {SubscriptionId}", subscriptionId);
                }
                else
                {
                    _responseDto.Result = subscription;
                    _responseDto.IsSuccess = subscription != null;
                    _responseDto.Message = subscription == null ? $"No record found for the subscription with Id - {subscriptionId}" : "";
                    _logger.LogInformation("No subscription found with Id: {SubscriptionId}", subscriptionId);
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while generating payment schedule for subscription Id: {SubscriptionId}", subscriptionId);
            }
            return _responseDto;
        }

        /// <summary>
        /// Generates payment schedules for a subscription, considering payout frequency, interest rate, and adjusting payment dates to Mondays if they fall on weekends.
        /// </summary>
        /// <param name="subscription">The subscription details.</param>
        /// <returns>A list of payment schedules.</returns>
        private List<PaymentScheduleDto> GenerateSchedules(TblSubscription subscription)
        {
            List<PaymentScheduleDto> schedules = new List<PaymentScheduleDto>();
            try
            {
                DateTime currentDate = subscription.DateOfInvestment;
                decimal remainingInterest = subscription.TotalInterest;
                decimal investmentAmount = subscription.InvestmentAmount;
                ResponseDto response = GetClientWithSubsByClientIdAsync(subscription.ClientId).Result;
                ClientMaster clientMaster = (ClientMaster)response.Result;

                // Access the Name property of the ClientMaster object
                string clientName = clientMaster != null ? clientMaster.Name : "";
                DateTime nextPaymentDate = new DateTime();
                while (nextPaymentDate < subscription.MaturityDate)
                {
                    PaymentScheduleDto schedule = new PaymentScheduleDto();

                    // Calculate payout amount
                    decimal payoutAmount = investmentAmount * subscription.PayoutFrequencyInterestRate / Convert.ToDecimal(100.0);

                    // Calculate the next payment date and adjust to Monday if it falls on a weekend
                    nextPaymentDate = currentDate.AddMonths(subscription.PayoutFrequency);

                    if (nextPaymentDate > currentDate)
                    {
                        // Adjust the day to the same day of the month as DateOfInvestment or the last valid day of the month
                        int daysInMonth = DateTime.DaysInMonth(nextPaymentDate.Year, nextPaymentDate.Month);
                        int targetDay = subscription.DateOfInvestment.Day <= daysInMonth ? subscription.DateOfInvestment.Day : daysInMonth;
                        nextPaymentDate = new DateTime(nextPaymentDate.Year, nextPaymentDate.Month, targetDay);

                        //nextPaymentDate = new DateTime(nextPaymentDate.Year, nextPaymentDate.Month, subscription.DateOfInvestment.Day);
                    }
                    if (nextPaymentDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        nextPaymentDate = nextPaymentDate.AddDays(2); // Adjust to Monday
                    }
                    else if (nextPaymentDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        nextPaymentDate = nextPaymentDate.AddDays(1); // Adjust to Monday
                    }

                    schedule.CreatedDate = DateTime.Now;
                    schedule.PayableAmount = payoutAmount;
                    schedule.InterestRate = subscription.PayoutFrequencyInterestRate;
                    //schedule.AmountToPaid = amountToPaid;
                    schedule.DueDate = nextPaymentDate;
                    schedule.Status = 0; // Set payment status as pending by default
                    schedule.InvestedAmount = subscription.InvestmentAmount;
                    schedule.SubscriptionId = subscription.SubscriptionId;
                    schedule.ClientId = subscription.ClientId;
                    schedule.Day = nextPaymentDate.DayOfWeek.ToString();
                    schedule.ClientName = clientName;
                    schedule.Guid = Guid.NewGuid();

                    schedules.Add(schedule);

                    // Update remaining interest and move to next payout date
                    remainingInterest -= payoutAmount;

                    //getting it back to investment date day
                    nextPaymentDate = currentDate.AddMonths(subscription.PayoutFrequency);
                    if (subscription.DateOfInvestment.Day > nextPaymentDate.Day)
                    {
                        int daysInMonth = DateTime.DaysInMonth(nextPaymentDate.Year, nextPaymentDate.Month);
                        int targetDay = subscription.DateOfInvestment.Day <= daysInMonth ? subscription.DateOfInvestment.Day : daysInMonth;
                        nextPaymentDate = new DateTime(nextPaymentDate.Year, nextPaymentDate.Month, targetDay);
                    }
                    currentDate = nextPaymentDate;
                }
            }
            catch
            {
                throw;
            }                                                                                                        
            return schedules;
        }

        public async Task<ResponseDto> GetPaymentScheduleBySubScriptionIdAsync(int subscriptionId)
        {
            try
            {
                List<TblPaymentSchedule> paymentSchedules = await _subscriptionRepo.GetPaymentSchedulesBySubScriptionIdAsync(subscriptionId);                
                var paySchedulesWithSubAndClient=_mapper.Map<List<PaymentScheduleDto>>(paymentSchedules);
                paySchedulesWithSubAndClient.ForEach(p => p.ClientName = paymentSchedules[0].ClientMaster.Name);
                _responseDto.Result = paySchedulesWithSubAndClient;
                _responseDto.IsSuccess = paySchedulesWithSubAndClient != null;
                _responseDto.Message = paySchedulesWithSubAndClient == null ? $"No record found for the subscription - {subscriptionId}" : "";

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching payment schedule for subscription Id: {SubscriptionId}", subscriptionId);
            }
            return _responseDto;
        }

		public async Task<ResponseDto> UpdateSubscriptionAsync(AddSubscriptionDto updateSubscriptionDto)
		{
			try
			{
				//this function returns 3 values
				//1. bool if maturity date is correct
				//2. difference in months
				//3. total installments or payouts
				var maturirtyInfo = CheckValidMaturity(updateSubscriptionDto);
				if (!maturirtyInfo.Item1)
				{
					throw new Exception("Please select correct maturity date. It should be in series of payout frequency.");
				}

				var userDetails = UserClaimsHelper.GetClaims(_contextAccessor.HttpContext.User.Identity as ClaimsIdentity);
                TblSubscription existingcription = await _subscriptionRepo.GetSubscriptionByIdAsync(updateSubscriptionDto.SubscriptionId);                       

                if(existingcription.IsPaymentScheduleAvailable == true)
                {
					throw new Exception("Subscription can not be updated after payment schedule.");
				}
                

				TblSubscription subscriptionToUpdate =  _mapper.Map<TblSubscription>(updateSubscriptionDto);
				subscriptionToUpdate.TotalInterest = maturirtyInfo.Item3 * subscriptionToUpdate.PayoutFrequencyInterestRate;
				subscriptionToUpdate.Tenure = maturirtyInfo.Item2;

				subscriptionToUpdate.ClientMasterClientId = existingcription.ClientId;
                subscriptionToUpdate.CreatedDate = existingcription.CreatedDate;
                subscriptionToUpdate.CreatedBy = existingcription.CreatedBy;
                subscriptionToUpdate.Guid = existingcription.Guid;
                subscriptionToUpdate.LastUpdatedBy = userDetails.UserLoginCode;
				_responseDto.Result = await _subscriptionRepo.UpdateSubscriptionAsync(subscriptionToUpdate);
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = $"Unable to update subscription. - {ex.Message}";
                _logger.LogError(ex, "An error occurred while updating subscription with Id: {SubscriptionId}", updateSubscriptionDto.SubscriptionId);
            }
            return _responseDto;
		}

		public async Task<ResponseDto> CreatePaymentScheduleForSubscriptionAsync(List<PaymentScheduleDto> paymentScheduleDtos)
		{
			try
			{
				var userDetails = UserClaimsHelper.GetClaims(_contextAccessor.HttpContext.User.Identity as ClaimsIdentity);

				List<TblPaymentSchedule> tblPaymentSchedules = _mapper.Map<List<TblPaymentSchedule>>(paymentScheduleDtos);
				tblPaymentSchedules.ForEach(tblPaymentSchedule => tblPaymentSchedule.CreatedBy = userDetails.UserName);
				_responseDto.Result = await _subscriptionRepo.CreatePaymentScheduleForSubscriptionAsync(tblPaymentSchedules);
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = $"Unable to create payment schedule for subscription. - {ex.Message}";
                _logger.LogError(ex, "An error occurred while creating payment schedules.");
            }
            return _responseDto;
		}

        public async Task<ResponseDto> GetAllDuedPaymentsFromPayScheduleAsync()
        {
            try
            {
                List<PaymentScheduleDto> paymentSchedules = await _subscriptionRepo.GetAllDuedPaymentsFromPayScheduleAsync();
                _responseDto.Result = paymentSchedules;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching due payments.");
            }
            return _responseDto;
        }
        public async Task<ResponseDto> GetAllTodaysPaymentsFromPayScheduleAsync()
        {
            try
            {
                List<PaymentScheduleDto> paymentSchedules = await _subscriptionRepo.GetAllTodaysPaymentsFromPayScheduleAsync();         
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
        public async Task<ResponseDto> GetAllFuturePaymentsFromPayScheduleAsync()
        {
            try
            {
                List<PaymentScheduleDto> paymentSchedules = await _subscriptionRepo.GetAllFuturePaymentsFromPayScheduleAsync();
                _responseDto.Result = paymentSchedules;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching future payments.");
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetAllPaymentsToProcessAsync()
        {
            try
            {
                var allPayments = await _subscriptionRepo.GetAllPaymentsToProcessAsync();

                _responseDto.Result = allPayments;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching all payments to process.");
            }

            return _responseDto;
        }

		public async Task<ResponseDto> GetPaymentHistories(DateTime startDate, DateTime endDate)
		{
			try
			{
				var paymentHistories = await _subscriptionRepo.GetPaymentHistories(startDate,endDate);

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

		public async Task<ResponseDto> UpdateMakersPaymentStatus(List<Guid> payments, int statusToUpdate,string comments)
		{
            try
            {
                var userDetails = UserClaimsHelper.GetClaims(_contextAccessor.HttpContext.User.Identity as ClaimsIdentity);
                await _subscriptionRepo.UpdateMakersPaymentStatus(payments, statusToUpdate,comments,userDetails.UserLoginCode);                  
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while updating makers payment status.");
            }
            return _responseDto;
		}

		public async Task<ResponseDto> UpdateManualPaymentStatus(UpdateManualPaymentStatusDto payment)
		{
            try
            {
				var userDetails = UserClaimsHelper.GetClaims(_contextAccessor.HttpContext.User.Identity as ClaimsIdentity);
                payment.PaidBy = userDetails.UserName;

				await _subscriptionRepo.UpdateManualPaymentStatus(payment);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess=false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while updating manual payment status.");
            }
            return _responseDto;
		}

		public async Task<ResponseDto> GetAllPaymentsForClient(int clientId)
		{
            try
            {
                var result = await _subscriptionRepo.GetAllClientsSubscriptionWithPayments(clientId);
                _responseDto.Result = result;         
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching payment for client Id: {ClientId}", clientId);
            }
            return _responseDto;
        }

		public async Task<ResponseDto> UpdateSubscriptionStatus(UpdateSubscriptionStatusRequestDto request)
		{
            try
            {
                await _subscriptionRepo.UpdateSubscriptionStatus(request);                
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while updating subscription status.");
            }

            return _responseDto;

		}

		public async Task<ResponseDto> GetBorrowLetterDetails(Guid sibsGuid)
		{
            try
            {
                _responseDto.Result = await _subscriptionRepo.GetBorrowLetterDetails(sibsGuid);                               
            }
            catch (Exception ex)
            {
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while getting borrow letter details.");
            }
            return _responseDto;
		}

		public async Task<ResponseDto> AddBorrowLetterDetails(BorrowLetterDetailsRequestDto borrowwLetter)
		{
            try
            {
                TblBorrowLetterDetails borrowLetter = new TblBorrowLetterDetails()
                {
                    ChequeDate = borrowwLetter.ChequeDate,
                    ChequeNo = borrowwLetter.ChequeNo,
                    SentDate = borrowwLetter.SentDate,
                    SubscriptionGuid = borrowwLetter.Guid,
                    SubscriptionId = borrowwLetter.SubscriptionId,
                    TrackingNo = borrowwLetter.TrackingNo,
                };
                await _subscriptionRepo.AddBorrowLetterDetails(borrowLetter,borrowwLetter.Status);
            }
            catch (Exception ex)
            {
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while adding borrow letter details.");
            }
            return _responseDto;

		}

		public async Task<ResponseDto> UpdateBorrowLetterDetails(BorrowLetterDetailsRequestDto borrowwLetter)
		{
			try
			{
				TblBorrowLetterDetails borrowLetter = new TblBorrowLetterDetails()
				{
					ChequeDate = borrowwLetter.ChequeDate,
					ChequeNo = borrowwLetter.ChequeNo,
					SentDate = borrowwLetter.SentDate,
					SubscriptionGuid = borrowwLetter.Guid,
					SubscriptionId = borrowwLetter.SubscriptionId,
					TrackingNo = borrowwLetter.TrackingNo,
				};
				await _subscriptionRepo.UpdateBorrowLetterDetails(borrowLetter, borrowwLetter.Status);
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while updating borrow letter details.");
            }
            return _responseDto;
		}

		public ResponseDto GetBorrowLetterDetailsToPrint(Guid sibsGuid)
		{
			try
			{
				_responseDto.Result = _subscriptionRepo.GetBorrowLetterDetailsToPrint(sibsGuid);
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching borrow letter details to print.");
            }
            return _responseDto;

		}
      


    }


}
