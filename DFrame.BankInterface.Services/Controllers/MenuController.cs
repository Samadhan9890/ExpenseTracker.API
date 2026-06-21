using ExpenseTracker.Services.Contracts.IContracts;
using ExpenseTracker.Services.Contracts.RequestResponseDto;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace ExpenseTracker.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuServices _menuservices;
        private readonly ILogger<MenuController> _logger;
        private readonly IResponseDto _responseDto;


        public MenuController(IMenuServices menuServices, ILogger<MenuController> logger, IResponseDto responseDto)
        {
            _menuservices = menuServices;
            _logger = logger;
            _responseDto = responseDto;
        }

        [HttpGet, Route("GetAccessibleMenus")]
        public IActionResult GetAccessibleMenus()
        {
            AccessibleMenu accessibleMenus = new AccessibleMenu();
            try
            {
                var userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);
                accessibleMenus = _menuservices.GetAccesibleMenus(userDetails.InternalUserId.ToString());

                _responseDto.Result = accessibleMenus;


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching accessible menu list. {ex}");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        [HttpGet, Route("GetAllAvailableMenus")]
        public async Task<IActionResult> GetAllAvailableMenus()
        {
            try
            {
                var accessibleMenus = await _menuservices.GetAllAvailableMenus();

                _responseDto.Result = accessibleMenus;
                _responseDto.IsSuccess = true;
                _responseDto.Message = $"All menu list.";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching all menu list. {ex}");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }


        [HttpGet, Route("GetAccessibleMenusByRoleId")]
        public async Task<IActionResult> GetAccessibleMenusByRoleId(string roleId)
        {
            try
            {
                var accessibleMenus = await _menuservices.GetAccesibleMenusByRoleId(roleId);

                _responseDto.Result = accessibleMenus;
                _responseDto.IsSuccess = true;
                _responseDto.Message = $"All menu list by role ID.";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching menu list. {ex}");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }
    }
}
