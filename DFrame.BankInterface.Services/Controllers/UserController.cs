using ExpenseTracker.Services.Contracts;
using ExpenseTracker.Services.Contracts.IContracts;
using ExpenseTracker.Services.Contracts.IContracts.BusinessUnitContracts;
using ExpenseTracker.Services.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Utilities;
using System.Security.Claims;


namespace ExpenseTracker.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserFactory _userFactory;
        //private readonly ILogger<MasterController> _logger;
        private ResponseDto _responseDto;
        public UserController(IUserFactory userFactory,ResponseDto responseDto)
        {
            _userFactory = userFactory;
            _responseDto = responseDto;
        }

        [HttpGet, Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            _responseDto = await _userFactory.GetUsersAsync();
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpGet, Route("GetUser")]
        public async Task<IActionResult> GetUser(int userId)
        {
            _responseDto = await _userFactory.GetUserAsync(userId);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpDelete ,Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            _responseDto = await _userFactory.DeleteUser(userId);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpPost, Route("CreateUser")]
        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            _responseDto = await _userFactory.CreateUser(userDto);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.Created : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpPut,Route("UpdateUser")]
        public async Task<IActionResult> Update(UserDto userDto)
        {
            _responseDto = await _userFactory.UpdateUser(userDto);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);
        }



        [HttpPost, Route("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel userPassword, string fields = "")
        {

            var userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);
            var _user = await _userFactory.UpdatePassword(Convert.ToInt32(userDetails.InternalUserId.ToString()), userPassword, fields);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);

        }


    }
}
