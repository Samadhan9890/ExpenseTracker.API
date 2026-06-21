using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Services.Contracts.RequestResponseDto;
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;
using ExpenseTracker.Services.Services;
using ExpenseTracker.Services.Services.IServices;
using ExpenseTracker.Services.Utilities;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using static ExpenseTracker.Services.Utilities.ConstantHelper.ConstantHelper;

namespace ExpenseTracker.Services.Controllers
{

	[Route("api/Subscription")]
	[ApiController]
	[Authorize]
	public class SubscriptionController : ControllerBase
	{
		private readonly ISubscriptionService _subscriptionService;
		private ResponseDto _responseDto;

		public SubscriptionController(ISubscriptionService subscriptionService,ResponseDto responseDto)
		{
			_subscriptionService = subscriptionService;
			_responseDto = responseDto;
		}

		[HttpGet]
		[Route("GetAllClientsWithSubs")]
		public async Task<ActionResult> GetClientsWithSubs()
		{
			ResponseDto response = await _subscriptionService.GetAllClientsWithSubscriptionsAsync();
			return StatusCode((int)HttpStatusCode.OK, response);
		}

		[HttpGet]
		[Route("GetAllClientsWithActionableSubs")]
		public async Task<ActionResult> GetAllClientsWithActionableSubs()
		{
			ResponseDto response = await _subscriptionService.GetAllClientsWithActionableSubscriptionsAsync();
			return StatusCode((int)HttpStatusCode.OK, response);
		}

		[HttpGet,Route("GetClientWithSubByClientId")]
		public async Task<ActionResult>	GetClientWithSubByClientId(int clientId)
		{
			ResponseDto response = await _subscriptionService.GetClientWithSubsByClientIdAsync(clientId);
			return StatusCode((int)HttpStatusCode.OK, response);

		}

		[HttpPost, Route("CreateSubscription")]
		public async Task<ActionResult> CreateSubscription(AddSubscriptionDto addSubscription)
		{

            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad request";
                return Ok(ModelState);
            }

            //if(addSubscription.MaturityDate <= DateTime.Now)
            //{
            //	_responseDto.IsSuccess = false;
            //	_responseDto.Message = "Maturity date should be greater than todays date.";
            //	return Ok(_responseDto);
            //}

            if (!IsPayoutDetailsCorrect(addSubscription))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Payout details are incorrect.";
                return Ok(_responseDto);
            }

            var result = await _subscriptionService.CreateSubscriptionAsync(addSubscription);

            return Ok(result);

        }

		[HttpPut, Route("UpdateSubscription")]
		public async Task<ActionResult> UpdateSubscription(AddSubscriptionDto updateSubscription)
		{

			if (!ModelState.IsValid)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = "Bad request";
				return Ok(ModelState);
			}

			//if (updateSubscription.MaturityDate <= DateTime.Now)
			//{
			//	_responseDto.IsSuccess = false;
			//	_responseDto.Message = "Maturity date should be greater than todays date.";
			//	return Ok(_responseDto);
			//}

			if (!IsPayoutDetailsCorrect(updateSubscription))
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = "Payout details are incorrect.";
				return Ok(_responseDto);
			}

			var result = await _subscriptionService.UpdateSubscriptionAsync(updateSubscription);

			return Ok(result);

		}

		private bool IsPayoutDetailsCorrect(AddSubscriptionDto addSubscription)
		{
			if (addSubscription.PayoutMethod == nameof(payoutMethodsEnum.Bank)
				&& (string.IsNullOrWhiteSpace(addSubscription.PayoutBankName) || 
				addSubscription.PayoutBankAccountNo == null || addSubscription.PayoutBankAccountNo == "0" ||
				string.IsNullOrWhiteSpace(addSubscription.PayoutBankIfscCode)))
			{
				return false;
			}
			else if(addSubscription.PayoutMethod == nameof(payoutMethodsEnum.Upi) 
				&& string.IsNullOrWhiteSpace(addSubscription.UpiId))
			{
				return false;
			}
			return true;
		}
        [HttpGet, Route("GetSubscriptionBySubscriptionId")]
        public async Task<ActionResult> GetSubscriptionBySubscriptionId(int subscriptionId)
        {
            ResponseDto response = await _subscriptionService.GetSubscriptionByIdAsync(subscriptionId);
            return StatusCode((int)HttpStatusCode.OK, response);

        }

        [HttpGet, Route("GeneratePaymentScheduleBySubScriptionId")]
        public async Task<ActionResult> GeneratePaymentScheduleBySubScriptionId(int subscriptionId)
        {
            ResponseDto response = await _subscriptionService.GeneratePaymentScheduleBySubScriptionIdAsync(subscriptionId);
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpGet, Route("GetPaymentScheduleBySubScriptionId")]
        public async Task<ActionResult> GetPaymentScheduleBySubScriptionId(int subscriptionId)
        {
            ResponseDto response = await _subscriptionService.GetPaymentScheduleBySubScriptionIdAsync(subscriptionId);
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        /// <summary>
        /// This endpoint creates payment schedules for subscriptions based on the provided data. 
        /// </summary>
        /// <param name="paymentSchedules">JSON object containing a list of payment schedule DTOs</param>
        /// <returns>It expects a list of payment schedule representing the payment schedules to be created for subscriptions.</returns>
        [HttpPost, Route("CreatePaymentScheduleForSubscription")]
        public async Task<ActionResult> CreatePaymentScheduleForSubscription(List<PaymentScheduleDto> paymentSchedules)
        {

            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad request";
                return Ok(ModelState);
            }
            // TODO: Implement logic to to do the validations if any

            var result = await _subscriptionService.CreatePaymentScheduleForSubscriptionAsync(paymentSchedules);

            return Ok(result);

        }

        /// <summary>
        /// This endpoint retrieves all due payments from the payment schedule.
        /// </summary>
        /// <returns>It returns a list of due payments along with their details.</returns>
        [HttpGet]
        [Route("GetAllDuedPaymentsFromPaySchedule")]
        public async Task<ActionResult> GetAllDuedPaymentsFromPaySchedule()
        {
            ResponseDto response = await _subscriptionService.GetAllDuedPaymentsFromPayScheduleAsync();
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        /// <summary>
        /// This endpoint retrieves all payments scheduled for today from the payment schedule.
        /// </summary>
        /// <returns>It returns a list of today's payments along with their details.</returns>
        [HttpGet]
        [Route("GetAllTodaysPaymentsFromPaySchedule")]
        public async Task<ActionResult> GetAllTodaysPaymentsFromPaySchedule()
        {
            ResponseDto response = await _subscriptionService.GetAllTodaysPaymentsFromPayScheduleAsync();
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        /// <summary>
        ///  This endpoint retrives all records within the date range from current date till 5 days in the future
        /// </summary>
        /// <returns>Return all future payment for next 5 days from current date.</returns>
        [HttpGet]
        [Route("GetAllFuturePaymentsFromPaySchedule")]
        public async Task<ActionResult> GetAllFuturePaymentsFromPaySchedule()
        {
            ResponseDto response = await _subscriptionService.GetAllFuturePaymentsFromPayScheduleAsync();
            return StatusCode((int)HttpStatusCode.OK , response);
        }

        /// <summary>
        ///  Retrieves all payments that need to be processed.
        /// </summary>
        /// <returns>Payments to process list</returns>
        [HttpGet]
        [Route("GetAllPaymentsToProcess")]
        public async Task<ActionResult> GetAllPaymentsToProcess()
        {
            ResponseDto response = await _subscriptionService.GetAllPaymentsToProcessAsync();
            return StatusCode((int)HttpStatusCode.OK, response);
        }


		/// <summary>
		///  Get payment histories
		/// </summary>
		/// <returns>Payments to process list</returns>
		[HttpGet]
		[Route("GetPaymentHistories")]
		public async Task<ActionResult> GetPaymentHistories(DateTime? startDate ,DateTime? endDate)
		{
			DateTime endDt = endDate == null  ? DateTime.Now: endDate.Value;
			DateTime startDt = startDate == null ? DateTime.Now.AddDays(-15) : startDate.Value;

			ResponseDto response = await _subscriptionService.GetPaymentHistories(startDt, endDt);
            return StatusCode((int)HttpStatusCode.OK, response);

        }

        [HttpPost]
		[Route("UpdateMakerPaymentStatus")]
		public async Task<ActionResult> UpdateMakerPaymentStatus([FromBody] UpdatePaymentStatusRequestDto request)
		{
			List<Guid> paymentsToUpdate = new();
			foreach (var pay in request.Payments)
			{
				if (Guid.TryParse(pay, out Guid guidOutput))
				{
					paymentsToUpdate.Add(guidOutput);
				}
			}

			var resp = await _subscriptionService.UpdateMakersPaymentStatus(paymentsToUpdate, request.StatusToUpdate,request.Comments);

			return StatusCode((int)HttpStatusCode.OK, resp);

		}

		[HttpPost]
		[Route("UpdateManualPaymentStatus")]
		public async Task<ActionResult> UpdateManualPaymentStatus([FromBody] UpdateManualPaymentStatusDto request)
		{			
			var resp = await _subscriptionService.UpdateManualPaymentStatus(request);
			return StatusCode((int)HttpStatusCode.OK, resp);

		}

		[HttpPost]
		[Route("UpdateSubscriptionStatus")]
		public async Task<ActionResult> UpdateSubscriptionStatus([FromBody] UpdateSubscriptionStatusRequestDto request)
		{
			var resp  = await _subscriptionService.UpdateSubscriptionStatus(request);
			return StatusCode((int)HttpStatusCode.OK, resp);
		}

		[HttpGet]
		[Route("GetBorrowLetterDetails")]
		public async Task<ActionResult> GetBorrowLetterDetails(Guid SubsGuid)
		{			
			ResponseDto response = await _subscriptionService.GetBorrowLetterDetails(SubsGuid);
			return StatusCode((int)HttpStatusCode.OK, response);
		}

		[HttpPost]
		[Route("AddBorrowLetterDetails")]
		public async Task<ActionResult> AddBorrowLetterDetails(BorrowLetterDetailsRequestDto borrowLetterrequest)
		{
			ResponseDto response = await _subscriptionService.AddBorrowLetterDetails(borrowLetterrequest);
			return StatusCode((int)HttpStatusCode.OK, response);
		}

		[HttpPost]
		[Route("UpdateBorrowLetterDetails")]
		public async Task<ActionResult> UpdateBorrowLetterDetails(BorrowLetterDetailsRequestDto borrowLetterrequest)
		{
			ResponseDto response = await _subscriptionService.UpdateBorrowLetterDetails(borrowLetterrequest);
			return StatusCode((int)HttpStatusCode.OK, response);
		}

		[HttpGet]
		[Route("GetBorrowLetterDetailsToPrint")]
		public ActionResult GetBorrowLetterDetailsToPrint(Guid SubsGuid)
		{
			ResponseDto response =  _subscriptionService.GetBorrowLetterDetailsToPrint(SubsGuid);
			return StatusCode((int)HttpStatusCode.OK, response);
		}

    }
}

