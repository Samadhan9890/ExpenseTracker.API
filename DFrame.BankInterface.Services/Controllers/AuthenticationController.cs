using ExpenseTracker.Services.Contracts.IContracts;
using ExpenseTracker.Services.Contracts.RequestResponseDto;
using ExpenseTracker.Services.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ExpenseTracker.Services.Utilities;
using System.Security.Claims;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ExpenseTracker.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationFactory _authenticationFactory;
        private readonly IResponseDto _responseDto;
        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(IAuthenticationFactory authenticationFactory, IResponseDto responseDto,ILogger<AuthenticationController> logger)
        {
           _authenticationFactory = authenticationFactory;
            _responseDto = responseDto;
            _logger = logger;
        }

        [HttpPost, Route("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticationRequestDto authenticationRequestDto)
        {
            try
            {
                _responseDto.Result = await _authenticationFactory.AuthenticateUser(authenticationRequestDto);
                _responseDto.Message = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to authenticate user - {ex.ToString()}");
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;			
			}
			return StatusCode((int)HttpStatusCode.OK, _responseDto);

		}

		[HttpPost, Route("ValidateOtp")]
		public async Task<IActionResult> ValidateOtp([FromBody] int otp)
		{
            ResponseDto responseDto = new();
			try
			{
				var userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);

				responseDto = await _authenticationFactory.ValidateOtp(otp,userDetails);
				
			}
			catch (Exception ex)
			{
                _logger.LogError($"Unable to validate otp - {ex.ToString()}");
                responseDto.Message = ex.Message;
				responseDto.IsSuccess = false;
			}
			return StatusCode((int)HttpStatusCode.OK, responseDto);

		}


	}
}
