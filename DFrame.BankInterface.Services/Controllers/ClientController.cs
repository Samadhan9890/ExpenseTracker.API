using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.ClientMaster;
using ExpenseTracker.Services.Services;
using ExpenseTracker.Services.Services.IServices;
using ExpenseTracker.Services.Utilities;
using System.Net;
using System.Security.Claims;

namespace ExpenseTracker.Services.Controllers
{
	[Authorize]
	[Route("api/client")]
	[ApiController]
	public class ClientController : ControllerBase
	{
		private readonly IClientMasterService _clientMasterService;
        private readonly IBusinessDevTeamService _businessDevTeamService;
        private ResponseDto _responseDto;
		private readonly IConfiguration _configurations;
		public ClientController(IClientMasterService clientMasterService, ResponseDto responseDto, IConfiguration configuration,IBusinessDevTeamService businessDevTeamService)
		{
			_clientMasterService = clientMasterService;
			_responseDto = responseDto;
			_configurations = configuration;
			_businessDevTeamService = businessDevTeamService;
		}
		[HttpPost]
		[Route("CreateClient")]
		public async Task<IActionResult> CreateClient([FromForm] ClientMasterRequestDto clientDto)
		{
            // Validate Aadhar and PAN by checking if they exist in the 'businessdevteam' table
            //bool isAadharOrPanExists = await _businessDevTeamService.CheckIfAadharOrPanExistsAsync(clientDto.AadharNo, clientDto.PanNo);

            //if (isAadharOrPanExists)
            //{
            //    // Aadhar or PAN is already in the 'businessdevteam' table, don't allow referral
            //    clientDto.ReferredBy = null;
            //    _responseDto.IsSuccess = false;
            //    _responseDto.Message = "Client already exists in the business development team or has been referred before.";
            //    return StatusCode((int)HttpStatusCode.OK, _responseDto); // Return conflict response
            //}

            clientDto.ReferredBy = clientDto.ReferredBy == "default" ? null : clientDto.ReferredBy;
			//Validate inputs
			if(clientDto.ProfileImageAttachment!= null)
			{
				Tuple<bool, string> profileValidation = ValidateAttachments(clientDto.ProfileImageAttachment,
				_configurations.GetValue<int>("AttachmentOptions:ProfileImage:maxSize"),
				_configurations.GetValue<string>("AttachmentOptions:ProfileImage:allowedFileTypes").Split(",").ToArray());
				if (!profileValidation.Item1)
				{
					_responseDto.IsSuccess = false;
					_responseDto.Message = profileValidation.Item2;
				}
			}
			

			if(clientDto.AadharAttachment!= null)
			{
				Tuple<bool, string> aadharValidation = ValidateAttachments(clientDto.AadharAttachment,
				_configurations.GetValue<int>("AttachmentOptions:Aadhar:maxSize"),
				_configurations.GetValue<string>("AttachmentOptions:Aadhar:allowedFileTypes").Split(",").ToArray());
				if (!aadharValidation.Item1)
				{
					_responseDto.IsSuccess = false;
					_responseDto.Message = aadharValidation.Item2;
				}
			}
			

			if (clientDto.PanAttachment != null)
			{
				Tuple<bool, string> panValidation = ValidateAttachments(clientDto.PanAttachment,
				_configurations.GetValue<int>("AttachmentOptions:Pan:maxSize"),
				_configurations.GetValue<string>("AttachmentOptions:Pan:allowedFileTypes").Split(",").ToArray());
				if (!panValidation.Item1)
				{
					_responseDto.IsSuccess = false;
					_responseDto.Message = panValidation.Item2;
				}
			}

			//validate clients banking details
			if (clientDto.LstClientBankingDetails != null && clientDto.LstClientBankingDetails.Count > 0) {

				foreach (var bank in clientDto.LstClientBankingDetails) { 
					
					if(bank.Mode == "bank" && (string.IsNullOrEmpty(bank.AccountHolderName) || string.IsNullOrEmpty(bank.BankName) || string.IsNullOrEmpty(bank.AccountNoOrUpiId) || string.IsNullOrEmpty(bank.IFSCCode)))
					{
						_responseDto.IsSuccess = false;
						_responseDto.Message = "Icomplete clients banking details filled.";
                        return StatusCode((int)HttpStatusCode.OK, _responseDto);
                    }
					else if(bank.Mode == "upi" && string.IsNullOrEmpty(bank.AccountNoOrUpiId))
					{
                        _responseDto.IsSuccess = false;
                        _responseDto.Message = "Icomplete clients banking details filled.";
                        return StatusCode((int)HttpStatusCode.OK, _responseDto);
                    }

                }
						
			}

			// Process the uploaded files here

			var userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);
			clientDto.CreatedBy = userDetails.UserLoginCode;



			_responseDto = await _clientMasterService.AddClientMasterAsync(clientDto);

			return StatusCode((int)HttpStatusCode.OK ,_responseDto);

		}

		[HttpGet]
		[Route("GetAllclients")]
		public async Task<IActionResult> GetAllClients()
		{
			_responseDto = await _clientMasterService.GetAllClientMastersAsync();

            return StatusCode((int)HttpStatusCode.OK, _responseDto);

        }

		[HttpGet]
		[Route("GetClientById")]
		public async Task<IActionResult> GetClientById(int ClientId)
		{
			_responseDto = await _clientMasterService.GetClientMasterByIdAsync(ClientId);

            return StatusCode((int)HttpStatusCode.OK, _responseDto);

        }

		[HttpPut]
		[Route("UpdateClient")]
		public async Task<IActionResult> UpdateClient(ClientMasterRequestDto clientDto)
		{
            clientDto.ReferredBy = clientDto.ReferredBy == "default" ? null : clientDto.ReferredBy;

            //Validate inputs
            if (clientDto.ProfileImageAttachment != null)
			{
				Tuple<bool, string> profileValidation = ValidateAttachments(clientDto.ProfileImageAttachment,
								_configurations.GetValue<int>("AttachmentOptions:ProfileImage:maxSize"),
								_configurations.GetValue<string>("AttachmentOptions:ProfileImage:allowedFileTypes").Split(",").ToArray());
				if (!profileValidation.Item1)
				{
					_responseDto.IsSuccess = false;
					_responseDto.Message = profileValidation.Item2;
				}
			}


			if (clientDto.AadharAttachment != null)
			{
				Tuple<bool, string> aadharValidation = ValidateAttachments(clientDto.AadharAttachment,
				_configurations.GetValue<int>("AttachmentOptions:Aadhar:maxSize"),
				_configurations.GetValue<string>("AttachmentOptions:Aadhar:allowedFileTypes").Split(",").ToArray());
				if (!aadharValidation.Item1)
				{
					_responseDto.IsSuccess = false;
					_responseDto.Message = aadharValidation.Item2;
				}
			}

			if (clientDto.PanAttachment != null)
			{
				Tuple<bool, string> panValidation = ValidateAttachments(clientDto.PanAttachment,
				_configurations.GetValue<int>("AttachmentOptions:Pan:maxSize"),
				_configurations.GetValue<string>("AttachmentOptions:Pan:allowedFileTypes").Split(",").ToArray());
				if (!panValidation.Item1)
				{
					_responseDto.IsSuccess = false;
					_responseDto.Message = panValidation.Item2;
				}
			}

			_responseDto = await _clientMasterService.UpdateClientMasterAsync(clientDto);
            return StatusCode((int)HttpStatusCode.OK, _responseDto);

        }


		private Tuple<bool, string> ValidateAttachments(IFormFile file, int maxSize, string[] allowedTypes)
		{

			if (file.Length / 1024 > maxSize)
			{
				return Tuple.Create(false, "File size exceeds allowed limit.");
			}
			else if (!allowedTypes.Any(c => c == Path.GetExtension(file.FileName).Replace(".", "")))
			{
				return Tuple.Create(false, "File type not allowed.");
			}
			else return Tuple.Create(true, "Ok");

		}

        [HttpGet("GetBankingDetailsByClientId/{clientId}")]
        public async Task<ActionResult> GetBankingDetailsByClientId(int clientId)
        {
            _responseDto = await _clientMasterService.GetBankingDetailsByClientIdAsync(clientId);

            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }
    }
}
