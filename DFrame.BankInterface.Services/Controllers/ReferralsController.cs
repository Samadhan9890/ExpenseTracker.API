using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Services.IServices;
using System.Net;

namespace ExpenseTracker.Services.Controllers
{
    [Authorize]
    [Route("api/referrals")]
    [ApiController]
    public class ReferralsController : ControllerBase
    {
        private readonly IReferralService _referralsService;
        private ResponseDto _responseDto;

        public ReferralsController(IReferralService referralService)
        {
            _referralsService = referralService;  
            _responseDto = new ResponseDto();

        }


        [HttpGet]
        [Route("GetBusinessDevTeamPerformance")]
        public async Task<IActionResult> GetBusinessDevTeamPerformance() {

            _responseDto = await _referralsService.GetAllBdPerformance();
            return StatusCode((int)HttpStatusCode.OK, _responseDto);

        }

        [HttpGet]
        [Route("GetBdHierarchyByClientId")]
        public async Task<IActionResult> GetBdHierarchyByClientId(int clientId)
        {

            _responseDto = await _referralsService.GetBdHierarchybyClientId(clientId);
            return StatusCode((int)HttpStatusCode.OK, _responseDto);

        }
    }
}
