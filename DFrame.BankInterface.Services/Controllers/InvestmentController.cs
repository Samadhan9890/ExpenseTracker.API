using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;
using ExpenseTracker.Services.Services;
using ExpenseTracker.Services.Services.IServices;
using ExpenseTracker.Services.Utilities;
using System.Net;
using System.Security.Claims;

namespace ExpenseTracker.Services.Controllers
{

    [Route("api/Investment")]
    [ApiController]
    public class InvestmentController : ControllerBase
    {
        private readonly IInvestmentService _investmentService;
        private ResponseDto _responseDto;

        public InvestmentController(IInvestmentService investmentService, ResponseDto responseDto)
        {
            _investmentService = investmentService;
            _responseDto = responseDto;
        }

        [HttpGet, Route("GetAllInvestments")]
        public async Task<ActionResult> GetAllInvestments()
        {
            ResponseDto response = await _investmentService.GetAllInvestments();
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        /// <summary>
        /// Creates a new investment record using the provided InvestmentsDto. 
        /// Performs necessary validations before saving the investment to the database.
        /// </summary>
        [HttpPost]
        [Route("CreateInvestment")]
        public async Task<ActionResult> CreateInvestment(InvestmentsDto investmentDto)
        {
            var userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);
            _responseDto.IsSuccess = false;

            // Check if the ModelState is valid for basic validations
            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad Request";
                return Ok(ModelState);
            }

            // Custom conditional validation: if IsMaturityBonusApplicable is true
            if (investmentDto.IsMaturityBonusApplicable)
            {
                if (string.IsNullOrEmpty(investmentDto.BonusTime))
                {
                    _responseDto.Message = "BonusTime must be greater than 0 when Is Maturity Bonus Applicable is true.";
                    return Ok(_responseDto);
                }

                if (investmentDto.BonusPercent <= 0)
                {
                    _responseDto.Message = "BonusPercent must be greater than 0 when Is Maturity Bonus Applicable is true.";
                    return Ok(_responseDto);
                }
            }
            // Custom conditional validation: if IsTdsApplicable is true
            if (investmentDto.IsTdsApplicable)
            {
                if (investmentDto.TdsPercent <= 0)
                {
                    _responseDto.Message = "TdsPercent must be greater than 0 and less than or equal to 100 when IsTdsApplicable is true.";
                    return Ok(_responseDto);
                }
            }
            else
            {
                // If Tds is not applicable, ensure TdsPercent is zero
                if (investmentDto.TdsPercent != 0)
                {
                    _responseDto.Message = "TdsPercent must be 0 when IsTdsApplicable is false.";
                    return Ok(_responseDto);
                }
            }
            // Perform additional manual validation if Mode is "Bank"
            //foreach (var bankingDetail in investmentDto.LstClientBankingDetails)
            //{
            //    if (bankingDetail.Mode.ToLower() == "bank")
            //    {
            //        if (string.IsNullOrEmpty(bankingDetail.BankName))
            //        {
            //            _responseDto.Message = "BankName is required if Mode is 'Bank'.";
            //            return Ok(_responseDto);
            //        }

            //        if (string.IsNullOrEmpty(bankingDetail.AccountHolderName))
            //        {
            //            _responseDto.Message = "AccountHolderName is required if Mode is 'Bank'.";
            //            return Ok(_responseDto);
            //        }

            //        if (string.IsNullOrEmpty(bankingDetail.IFSCCode))
            //        {
            //            _responseDto.Message = "IFSCCode is required if Mode is 'Bank'.";
            //            return Ok(_responseDto);
            //        }
            //    }
            //}

            // Conditional Validation for InvestmentReceivedDetailDto
            foreach (var receivedDetail in investmentDto.LstInvestmentReceivedDetails)
            {
                if (receivedDetail.Mode.ToLower() == "bank")
                {
                    if (string.IsNullOrEmpty(receivedDetail.BankName))
                    {
                        _responseDto.Message = "BankName is required if Mode is 'Bank' in InvestmentReceivedDetail.";
                        return Ok(_responseDto);
                    }
                }
            }

            try
            {
                investmentDto.CreatedBy = userDetails.UserLoginCode;
                // Proceed to create the investment if all validations pass
                _responseDto = await _investmentService.CreateInvestmentAsync(investmentDto);
            }
            catch (Exception ex)
            {
                // Handle any exception that occurs during processing
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex}, Error occurred while creating investment";
            }
            return Ok(_responseDto);
        }

        /// <summary>
        /// Updates an existing investment record by applying the changes from the provided InvestmentsDto. 
        /// Only the modified fields are updated in the database.
        /// </summary>
        [HttpPut]
        [Route("UpdateInvestment")]
        public async Task<ActionResult> UpdateInvestment(UpdateInvestmentsDto investmentDto)
        {
            var userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);
            _responseDto.IsSuccess = false;

            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad Request";
                return Ok(ModelState);
            }

            // Perform conditional validations as in CreateInvestment
            if (investmentDto.IsMaturityBonusApplicable && (string.IsNullOrEmpty(investmentDto.BonusTime) || investmentDto.BonusPercent <= 0))
            {
                _responseDto.Message = "BonusTime and BonusPercent must be valid when Maturity Bonus is applicable.";
                return Ok(_responseDto);
            }

            if (investmentDto.IsTdsApplicable && (investmentDto.TdsPercent <= 0 || investmentDto.TdsPercent > 100))
            {
                _responseDto.Message = "TdsPercent must be greater than 0 and less than or equal to 100 when TDS is applicable.";
                return Ok(_responseDto);
            }

            //foreach (var bankingDetail in investmentDto.LstClientBankingDetails)
            //{
            //    if (bankingDetail.Mode.ToLower() == "bank" && (string.IsNullOrEmpty(bankingDetail.BankName) ||
            //                                                   string.IsNullOrEmpty(bankingDetail.AccountHolderName) ||
            //                                                   string.IsNullOrEmpty(bankingDetail.IFSCCode)))
            //    {
            //        _responseDto.Message = "BankName, AccountHolderName, and IFSCCode are required if Mode is 'Bank'.";
            //        return Ok(_responseDto);
            //    }
            //}

            //foreach (var receivedDetail in investmentDto.LstInvestmentReceivedDetails)
            //{
            //    if (receivedDetail.Mode.ToLower() == "bank" && string.IsNullOrEmpty(receivedDetail.BankName))
            //    {
            //        _responseDto.Message = "BankName is required in InvestmentReceivedDetail if Mode is 'Bank'.";
            //        return Ok(_responseDto);
            //    }
            //}

            try
            {
                //investmentDto.UpdatedBy = userDetails.UserLoginCode;
                _responseDto = await _investmentService.UpdateInvestmentAsync(investmentDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex}, Error occurred while updating investment";
            }
            return Ok(_responseDto);
        }

        /// <summary>
        /// Updates an existing investment received details record using the provided InvestmentReceivedDetailDto. 
        /// Only modified fields are updated while keeping the unchanged fields intact.
        /// </summary>
        [HttpPut]
        [Route("UpdateInvestmentReceivedDetails")]
        public async Task<ActionResult> UpdateInvestmentReceivedDetails(InvestmentReceivedDetailDto receivedDetailDto)
        {
            _responseDto.IsSuccess = false;

            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad Request";
                return Ok(ModelState);
            }

            if (receivedDetailDto.Mode.ToLower() == "bank" && string.IsNullOrEmpty(receivedDetailDto.BankName))
            {
                _responseDto.Message = "BankName is required if Mode is 'Bank'.";
                return Ok(_responseDto);
            }

            try
            {
                _responseDto = await _investmentService.UpdateInvestmentReceivedDetailsAsync(receivedDetailDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex}, Error occurred while updating investment received details.";
            }
            return Ok(_responseDto);
        }

        /// <summary>
        /// Updates an existing client banking detail record using the provided ClientBankingDetailsDto. 
        /// Only the changed properties are updated in the database.
        /// </summary>
        [HttpPut]
        [Route("UpdateClientBankingDetail")]
        public async Task<ActionResult> UpdateClientBankingDetail(ClientBankingDetailsDto bankingDetailDto)
        {
            _responseDto.IsSuccess = false;

            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad Request";
                return Ok(ModelState);
            }

            if (bankingDetailDto.Mode.ToLower() == "bank" && (string.IsNullOrEmpty(bankingDetailDto.BankName) ||
                                                              string.IsNullOrEmpty(bankingDetailDto.AccountHolderName) ||
                                                              string.IsNullOrEmpty(bankingDetailDto.IFSCCode)))
            {
                _responseDto.Message = "BankName, AccountHolderName, and IFSCCode are required if Mode is 'Bank'.";
                return Ok(_responseDto);
            }

            try
            {
                _responseDto = await _investmentService.UpdateClientBankingDetailAsync(bankingDetailDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex}, Error occurred while updating client banking detail.";
            }
            return Ok(_responseDto);
        }


        [HttpGet, Route("GetClientWithInvestmentByClientId")]
        public async Task<ActionResult> GetClientWithInvestmentByClientId(int clientId)
        {
            ResponseDto response = await _investmentService.GetClientWithInvestmentByClientIdAsync(clientId);
            return StatusCode((int)HttpStatusCode.OK, response);
        }


        [HttpGet, Route("GetInvestmentById")]
        public async Task<ActionResult> GetInvestmentById(int investmentId)
        {
            ResponseDto response = await _investmentService.GetInvestmentByIdAsync(investmentId);
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpGet, Route("GenerateSplPaymentSchedule")]
        public async Task<ActionResult> GenerateSplPaymentSchedule(int investmentId)
        {
            ResponseDto response = await _investmentService.GenerateSplPaymentSchedule(investmentId);
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpPost("CreateInvestmentPaymentSchedules")]
        public async Task<IActionResult> CreateInvestmentPaymentSchedules([FromBody] List<SplPaymentScheduleDto> schedulesDto)
        {
            
            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad Request.";
                return Ok(ModelState);
            }

            _responseDto = await _investmentService.CreateInvestmentPaymentSchedulesAsync(schedulesDto);
            return Ok(_responseDto);
        }

        [HttpGet("GetPaymentScheduleByInvestmentId")]
        public async Task<IActionResult> GetPaymentScheduleByInvestmentId(int investmentId)
        {
            _responseDto = await _investmentService.GetSplPaymentScheduleByInvId(investmentId);
            return Ok(_responseDto);
        }

        [HttpGet]
        [Route("GetAllTodaysPaymentsFromInvestmentPaySchedule")]
        public async Task<ActionResult> GetAllTodaysPaymentsFromInvestmentPaySchedule()
        {
            ResponseDto response = await _investmentService.GetAllTodaysPaymentsFromInvestPayScheduleAsync();
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("GetAllDuedSplPaymentsFromInvestmentPaySchedule")]
        public async Task<ActionResult> GetAllDuedSplPaymentsFromInvestmentPaySchedule()
        {
            ResponseDto response = await _investmentService.GetAllDuedSplPaymentsFromInvestmentPayScheduleAsync();
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("GetInvestmentPaymentHistories")]
        public async Task<ActionResult> GetInvestmentPaymentHistories(DateOnly? startDate, DateOnly? endDate)
        {
            
            DateOnly endDt = endDate == null ? DateOnly.FromDateTime(DateTime.Now) : endDate.Value;
            DateOnly startDt = startDate == null ? DateOnly.FromDateTime(DateTime.Now.AddDays(-15)): startDate.Value;

            _responseDto = await _investmentService.GetInvestmentPaymentHistoriesAsync(startDt, endDt);
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        [HttpGet]
        [Route("GetAllInvestmentPaymentsToProcess")]
        public async Task<ActionResult> GetAllInvestmentPaymentsToProcess()
        {
            ResponseDto response = await _investmentService.GetAllInvestmentPaymentsToProcessAsync();
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("UpdateMakerInvestmentPaymentScheduleStatus")]
        public async Task<ActionResult> UpdateMakerInvestmentPaymentScheduleStatus([FromBody] UpdateInvestmentPaymentStatusRequestDto request)
        {
            List<Guid> paymentsToUpdate = new();
            foreach (var pay in request.Payments)
            {
                if (Guid.TryParse(pay, out Guid guidOutput))
                {
                    paymentsToUpdate.Add(guidOutput);
                }
            }

            var resp = await _investmentService.UpdateMakerInvestmentPaymentScheduleStatusAsync(paymentsToUpdate, request.StatusToUpdate, request.Comments);

            return StatusCode((int)HttpStatusCode.OK, resp);

        }

        [HttpPost]
        [Route("UpdateManualInvestmentPaymentScheduleStatus")]
        public async Task<ActionResult> UpdateManualInvestmentPaymentScheduleStatus([FromBody] UpdateManualInvestmentPaymentStatusDto request)
        {
            var resp = await _investmentService.UpdateManualInvestmentPaymentScheduleStatusAsync(request);
            return StatusCode((int)HttpStatusCode.OK, resp);

        }


        #region ExternalPayments

        [HttpGet]
        [Route("GetAllExternalPayments")]
        public async Task<ActionResult> GetAllExternalPayments()
        {
            var response = await _investmentService.GetAllExternalPaymentsAsync();
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("GetExternalPaymentById")]
        public async Task<ActionResult> GetExternalPaymentById(int id)
        {
            var response = await _investmentService.GetExternalPaymentByIdAsync(id);
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("AddExternalPayment")]
        public async Task<ActionResult> AddExternalPayment([FromBody] ExternalPaymentsDto paymentDto)
        {
            var response = await _investmentService.AddExternalPaymentAsync(paymentDto);
            return StatusCode((int)HttpStatusCode.Created, response);
        }

        [HttpPut]
        [Route("UpdateExternalPayment")]
        public async Task<ActionResult> UpdateExternalPayment([FromBody] ExternalPaymentsDto paymentDto)
        {
            var response = await _investmentService.UpdateExternalPaymentAsync(paymentDto);
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpDelete]
        [Route("DeleteExternalPayment")]
        public async Task<ActionResult> DeleteExternalPayment(int id)
        {
            var response = await _investmentService.DeleteExternalPaymentAsync(id);
            return StatusCode((int)HttpStatusCode.OK, response);
        }
        #endregion



        [HttpPost]
        [Route("CreateInvestmentReceivedDetails")]
        public async Task<ActionResult> CreateInvestmentReceivedDetails([FromForm] InvestmentReceivedDetailDto receivedDetailDto,
                                                        [FromForm] IFormFile? investmentAttachment)
        {
            _responseDto.IsSuccess = false;

            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad Request";
                return Ok(ModelState);
            }

            if (receivedDetailDto.Mode.ToLower() == "bank" && string.IsNullOrEmpty(receivedDetailDto.BankName))
            {
                _responseDto.Message = "BankName is required if Mode is 'Bank'.";
                return Ok(_responseDto);
            }

            byte[] compressedAttachment = [];
            if (investmentAttachment != null && investmentAttachment.Length > 0)
            {

                using (var memoryStream = new MemoryStream())
                {
                    await investmentAttachment.CopyToAsync(memoryStream);
                    compressedAttachment = memoryStream.ToArray();
                }
            }
            var userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);

            var receivedDetail = new InvestmentReceivedDetailDto
            {
                InvestmentId = receivedDetailDto.InvestmentId,
                InvestmentGuid = receivedDetailDto.InvestmentGuid,
                Mode = receivedDetailDto.Mode,
                BankName = receivedDetailDto.BankName,
                AccountNumberOrUpiId = receivedDetailDto.AccountNumberOrUpiId,
                Amount = receivedDetailDto.Amount,
                Comments = receivedDetailDto.Comments,
                AddedBy = receivedDetailDto.AddedBy,
                ReceivedDate = receivedDetailDto.ReceivedDate,
                Status = true,
                InvestmentAttachment = compressedAttachment,
            };

            try
            {
                _responseDto = await _investmentService.CreateInvestmentReceivedDetailsAsync(receivedDetail);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex}, Error occurred while adding investment received details.";
            }
            return Ok(_responseDto);
        }



        [HttpPost]
        [Route("AddInvestmentReceivedDetailsAttachment")]
        public async Task<ActionResult> AddInvestmentReceivedDetailsAttachment([FromForm] int id,
                                                       [FromForm] IFormFile? investmentAttachment)
        {
            _responseDto.IsSuccess = false;
            try
            {
                _responseDto = await _investmentService.AddInvestmentRecAttachement(id, investmentAttachment);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex}, Error occurred while updating investment received details.";
            }
            return Ok(_responseDto);
        }

        /// <summary>
        /// create client banking detail record using the provided ClientBankingDetailsDto. 
        /// </summary>
        [HttpPost]
        [Route("CreateClientBankingDetail")]
        public async Task<ActionResult> CreateClientBankingDetail(ClientBankingDetailsDto bankingDetailDto)
        {
            _responseDto.IsSuccess = false;

            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad Request";
                return Ok(ModelState);
            }

            if (bankingDetailDto.Mode.ToLower() == "bank" && (string.IsNullOrEmpty(bankingDetailDto.BankName) ||
                                                              string.IsNullOrEmpty(bankingDetailDto.AccountHolderName) ||
                                                              string.IsNullOrEmpty(bankingDetailDto.IFSCCode)))
            {
                _responseDto.Message = "BankName, AccountHolderName, and IFSCCode are required if Mode is 'Bank'.";
                return Ok(_responseDto);
            }

            try
            {
                _responseDto = await _investmentService.CreateClientBankingDetailAsync(bankingDetailDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex}, Error occurred while adding client banking detail.";
            }
            return Ok(_responseDto);
        }


        [HttpPut]
        [Route("DeleteInvestmentReceivedDetails")]
        public async Task<ActionResult> DeleteInvestmentReceivedDetails(int id)
        {
            try
            {
                _responseDto = await _investmentService.DeleteInvestmentReceivedDetailsAsync(id);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex}, Error occurred while deleting Investment Received Details  .";
            }
            return Ok(_responseDto);
        }

        [HttpPut]
        [Route("DeleteClientBankingDetail")]
        public async Task<ActionResult> DeleteClientBankingDetail(int id)
        {
            try
            {
                _responseDto = await _investmentService.DeleteClientBankingDetailAsync(id);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex}, Error occurred while deleting client banking detail.";
            }
            return Ok(_responseDto);
        }

        [HttpPost]
        [Route("SaveInvestmentBorrowLetterDetails")]
        public async Task<ActionResult> SaveInvestmentBorrowLetterDetails(InvestmentBorrowLetterRequestDto borrowLetterrequest)
        {
            ResponseDto response = await _investmentService.SaveInvestmentBorrowLetterDetailsAsync(borrowLetterrequest);
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("UpdateInvestmentStatus")]
        public async Task<ActionResult> UpdateInvestmentStatus([FromBody] UpdateSubscriptionStatusRequestDto request)
        {
            var userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);
            request.ActionBy = userDetails.UserName;
            var resp = await _investmentService.UpdateInvestmentStatus(request);
            return StatusCode((int)HttpStatusCode.OK, resp);
        }

        [HttpGet]
        [Route("GetAllClientsWithActionableInvestments")]
        public async Task<ActionResult> GetAllClientsWithActionableInvestments()
        {
            ResponseDto response = await _investmentService.GetAllClientsWithActionableInvestmentsAsync();
            return StatusCode((int)HttpStatusCode.OK, response);
        }
    }
}
