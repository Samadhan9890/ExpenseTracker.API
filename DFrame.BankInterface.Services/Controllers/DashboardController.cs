
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Services.Contracts.RequestResponseDto;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Services.IServices;
using System.Net;

namespace ExpenseTracker.Services.Controllers
{

	[Route("api/dashboard")]
	[ApiController]
	[Authorize]
	public class DashboardController : ControllerBase
	{	
		private readonly IClientMasterService _clientMasterService;
		private readonly ISubscriptionService _subscriptionService;
		private readonly IDashboardService _dboardService;
		private readonly IAuditTrailService _auditTrailService;


		public DashboardController(
			IClientMasterService clientMasterService,
			ISubscriptionService subscriptionService,IDashboardService dashboardService
			,IAuditTrailService auditTrailService)
        {
            _clientMasterService = clientMasterService;
			_subscriptionService= subscriptionService;
			_dboardService = dashboardService;
			_auditTrailService = auditTrailService;
        }

        [HttpGet,Route("GetClientBasicDetailsforDashb")]
		public async Task<ActionResult> GetClientBasicDetailsforDashb(int clientId)
		{
			var response = await _clientMasterService.GetClientMasterByIdAsync(clientId);
			return StatusCode( (int)HttpStatusCode.OK,response );
		}

		[HttpGet, Route("GetAllPaymentsByClientId")]
		public async Task<ActionResult> GetAllPaymentsByClientId(int clientId)
		{
			var response = await _subscriptionService.GetAllPaymentsForClient(clientId);
			return StatusCode((int)HttpStatusCode.OK, response);
		}

		[HttpGet, Route("GetDashbData")]
		public async Task<ActionResult> GetDashboardData(string? type,string? periodFrom, string? periodTo)
		{
			
			type ??= "All";
			DateOnly? fromDate = null;
			if (!string.IsNullOrEmpty(periodFrom))
			{
				if (DateOnly.TryParse(periodFrom, out var result))
				{
					fromDate = result;
				}
				else
				{
					throw new FormatException("Invalid date format for periodFrom.");
				}
			}

			DateOnly? toDate = null;
			if (!string.IsNullOrEmpty(periodTo))
			{
				if (DateOnly.TryParse(periodTo, out var result))
				{
					toDate = result;
				}
				else
				{
					throw new FormatException("Invalid date format for periodTo.");
				}
			}
			var response = _dboardService.GetDashboardData(type, fromDate,toDate);
			return StatusCode((int)HttpStatusCode.OK, response);
		}

        [HttpGet, Route("GetNcAuditTrail")]
        public async Task<ActionResult> GetNcAuditTrail(int unqId,string module)
        {
            var response = await _auditTrailService.GetNcAuditTrailAsync(unqId,module);
            return StatusCode((int)HttpStatusCode.OK, response);
        }
    }
}
