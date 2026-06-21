using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.ClientMaster;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;
using ExpenseTracker.Services.Services;
using ExpenseTracker.Services.Services.IServices;
using System.Net;

namespace ExpenseTracker.Services.Controllers
{
    [Authorize]
    [Route("api/businessdevteam")]
    [ApiController]
    public class BusinessDevTeamController : ControllerBase
    {
        private readonly IBusinessDevTeamService _businessDevTeamService;
        private ResponseDto _responseDto;

        public BusinessDevTeamController(IBusinessDevTeamService businessDevTeamService, ResponseDto responseDto)
        {
            _businessDevTeamService = businessDevTeamService;
            _responseDto = responseDto;
        }

        /// <summary>
        /// Retrieves all business development team members.
        /// </summary>
        /// <returns>An IActionResult containing a list of business development team members and their details. If an error occurs, returns an error message.</returns>
        [HttpGet]
        [Route("GetAllBusineesDevTeam")]
        public async Task<IActionResult> GetAllBusineesDevTeam()
        {
            _responseDto = await _businessDevTeamService.GetAllBusineesDevTeamAsync();
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        [HttpGet]
        [Route("GetAllBDTeam")]
        public async Task<IActionResult> GetBDTeam()
        {
            _responseDto = await _businessDevTeamService.GetAllBDAsync();
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }


        /// <summary>
        /// Retrieves the details of a specific business development team member by their ID.
        /// </summary>
        /// <param name="BusineesDevTeamMemberId">The ID of the business development team member to retrieve.</param>
        /// <returns>An IActionResult containing the details of the specified business development team member. If an error occurs, returns an error message.</returns>
        [HttpGet]
        [Route("GetBusineesDevTeamMemberById")]
        public async Task<IActionResult> GetBusineesDevTeamMemberById(int BusineesDevTeamMemberId)
        {
            _responseDto = await _businessDevTeamService.GetBusineesDevTeamMemberByIdAsync(BusineesDevTeamMemberId);

            return StatusCode((int)HttpStatusCode.OK, _responseDto);

        }

        /// <summary>
        /// Creates a new business development team member.
        /// </summary>
        /// <param name="addBusinessDevTeam">The data transfer object containing details of the business development team member to be created.</param>
        /// <returns>An ActionResult indicating the result of the operation. Returns the created business development team member details if successful; otherwise, returns an error message.</returns>
        [HttpPost, Route("CreateBusinessDevTeam")]
        public async Task<ActionResult> CreateBusinessDevTeam(BusinessDevTeamDTO addBusinessDevTeam)
        {

            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad request";
                return Ok(ModelState);
            }
            _responseDto = await _businessDevTeamService.CreateBusinessDevTeamAsync(addBusinessDevTeam);
            return StatusCode((int)HttpStatusCode.OK, _responseDto);

        }

        /// <summary>
        /// Updates the details of an existing business development team member.
        /// Soft deletes a business development team member by updating their status to inactive.
        /// </summary>
        /// <param name="updateBusinessDevTeam">The data transfer object containing the updated details of the business development team member.</param>
        /// <returns>An IActionResult indicating the result of the operation. Returns success if the update is successful; otherwise, returns an error message.</returns>
        [HttpPut]
        [Route("UpdateBusinessDevTeam")]
        public async Task<IActionResult> UpdateBusinessDevTeam(BusinessDevTeamDTO updateBusinessDevTeam)
        {
            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad request";
                return Ok(ModelState);
            }
            _responseDto = await _businessDevTeamService.UpdateBusinessDevTeamAsync(updateBusinessDevTeam);
            return StatusCode((int)HttpStatusCode.OK, _responseDto);

        }

        [HttpPost]
        [Route("CreateBusinessDevTeamBankingDetails")]
        public async Task<ActionResult> CreateBusinessDevTeamBankingDetails(List<ClientBankingDetailsDto> bankingDetailsDtos)
        {
            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad request";
                return Ok(ModelState);
            }

            // Iterate through each banking detail DTO to perform validation
            foreach (var bankingDetailDto in bankingDetailsDtos)
            {
                // Validate each DTO's Mode and required fields
                if (bankingDetailDto.Mode.ToLower() == "bank" &&
                    (string.IsNullOrEmpty(bankingDetailDto.BankName) ||
                     string.IsNullOrEmpty(bankingDetailDto.AccountHolderName) ||
                     string.IsNullOrEmpty(bankingDetailDto.IFSCCode)))
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = $"BankName, AccountHolderName, and IFSCCode are required if Mode is 'Bank'.";
                    return Ok(_responseDto);
                }
            }


            // Call the service layer to process the banking details
            _responseDto = await _businessDevTeamService.CreateBusinessDevTeamBankingDetailsAsync(bankingDetailsDtos);

            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        [HttpPut]
        [Route("UpdateBusinessDevTeamBankingDetails")]
        public async Task<ActionResult> UpdateBusinessDevTeamBankingDetails(ClientBankingDetailsDto bankingDetailDto)
        {
            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Bad request";
                return Ok(ModelState);
            }

            if (bankingDetailDto.Mode.ToLower() == "bank" && (string.IsNullOrEmpty(bankingDetailDto.BankName) ||
                                                            string.IsNullOrEmpty(bankingDetailDto.AccountHolderName) ||
                                                            string.IsNullOrEmpty(bankingDetailDto.IFSCCode)))
            {
                _responseDto.Message = "BankName, AccountHolderName, and IFSCCode are required if Mode is 'Bank'.";
                return Ok(_responseDto);
            }

            // Call the service layer to process the banking details update
            _responseDto = await _businessDevTeamService.UpdateBusinessDevTeamBankingDetailAsync(bankingDetailDto);

            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }
    }
}
