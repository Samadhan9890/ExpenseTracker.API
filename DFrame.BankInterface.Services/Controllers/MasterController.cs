using ExpenseTracker.Services.Contracts;
using ExpenseTracker.Services.Contracts.IContracts;
using ExpenseTracker.Services.Contracts.IContracts.BusinessUnitContracts;
using ExpenseTracker.Services.Contracts.RequestResponseDto;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using ExpenseTracker.Services.Services;

namespace ExpenseTracker.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly ILogger<MasterController> _logger;
        private ResponseDto _responseDto;
        private readonly IBusinessUnit _businessUnit;
        private readonly IBankMasterService _bankMasterService;
        private readonly IRoleMasterService _roleMasterService;
        private readonly ILocationMasterService _locationMasterService;
        private readonly IDepartmentMasterService _DepartmentMasterService;
        private readonly IMenuServices _menuServices;
        private readonly IPlanMasterService _planMasterService;


        public MasterController(ILogger<MasterController> logger,
                                ResponseDto responseDto,
                                IBusinessUnit businessUnit,
                                IBankMasterService bankMasterService,
                                IRoleMasterService roleMasterService,
                                IMenuServices menuServices,
                                ILocationMasterService locationMasterService,
                                IDepartmentMasterService departmentMasterService,
                                IPlanMasterService planMasterService
            )
        {
            _logger = logger;
            _responseDto = responseDto;
            _businessUnit = businessUnit;
            _bankMasterService = bankMasterService;
            _roleMasterService = roleMasterService;
            _menuServices = menuServices;
            _locationMasterService = locationMasterService;
            _DepartmentMasterService = departmentMasterService;
            _planMasterService = planMasterService;
        }

        #region BU Master
        [HttpGet, Route("GetAllBU")]
        public async Task<IActionResult> GetAllBU()
        {
            _responseDto = await _businessUnit.GetAllBU();
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        [HttpGet, Route("GetBUbyId")]
        public async Task<IActionResult> GetBU(int buid)
        {
            _responseDto = await _businessUnit.GetBU(buid);
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        [HttpDelete, Route("DeleteBU")]
        public async Task<IActionResult> DeleteBU(int buid)
        {
            _responseDto = await _businessUnit.DeleteBU(buid);
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        [HttpPost, Route("CreateBU")]
        public async Task<IActionResult> CreateBU(BusinessUnitRequestDto businessUnitRequestDto)
        {
            _responseDto = await _businessUnit.CreateBusinessUnit(businessUnitRequestDto);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.Created : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpPut, Route("UpdateBU")]
        public async Task<IActionResult> UpdateBU(BusinessUnitRequestDto businessUnitRequestDto)
        {
            _responseDto = await _businessUnit.UpdateBU(businessUnitRequestDto);
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }
        #endregion BU Master

        #region Bank Master
        [HttpGet, Route("GetAllBankMasters")]
        public async Task<IActionResult> GetAllBankMasters()
        {
            _responseDto = await _bankMasterService.GetAllBankMastersAsync();
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpPost, Route("CreateBankMaster")]
        public async Task<IActionResult> CreateBankMaster([FromBody] BankMasterDto bank)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _responseDto = await _bankMasterService.AddBankMasterAsync(bank);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.Created : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpDelete, Route("DeleteBankMaster")]
        public async Task<ActionResult> DeleteBankMaster(int bankId)
        {
            await _bankMasterService.DeleteBankMasterAsync(bankId);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);

        }


        [HttpGet, Route("GetBankMasterById")]
        public async Task<ActionResult> GetBankMasterById(int bankId)
        {
            await _bankMasterService.GetBankMasterByIdAsync(bankId);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);

        }

        [HttpPut, Route("UpdateBankMaster")]
        public async Task<ActionResult> UpdateBankMaster([FromBody] BankMasterDto bank)
        {
            await _bankMasterService.UpdateBankMasterAsync(bank);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);

        }

        #endregion Bank Master

        #region Role Master
        [HttpGet, Route("GetAllRoleMasters")]
        public async Task<IActionResult> GetAllRoles()
        {
            _responseDto = await _roleMasterService.GetAllRoles();
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        [HttpGet, Route("GetRoleById")]
        public async Task<IActionResult> GetRoleById(int roleId)
        {
            _responseDto = await _roleMasterService.GetRoleById(roleId);
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        [HttpPost, Route("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto roleDto, string? roleMenuAccess)
        {
            try
            {
                _responseDto = await _roleMasterService.AddRole(roleDto);
                RoleDto createdRole = (RoleDto)_responseDto?.Result;
                var menu = await _menuServices.AddUpdateMenuAccess(createdRole.RoleId, roleMenuAccess);
                return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.Created : (int)HttpStatusCode.InternalServerError, _responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while creating role. {ex}");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        [HttpPut, Route("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDto roleDto, string? roleMenuAccess)
        {
            try
            {
                _responseDto = await _roleMasterService.UpdateRole(roleDto);
                RoleDto updatedRole = (RoleDto)_responseDto?.Result;
                var menu = await _menuServices.AddUpdateMenuAccess(updatedRole.RoleId, roleMenuAccess);
                return StatusCode((int)HttpStatusCode.OK, _responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating role. {ex}");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        [HttpDelete, Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            _responseDto = await _roleMasterService.DeleteRole(roleId);
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }
        #endregion Role Master

        #region Location Master
        [HttpGet, Route("GetAllLocationMaster")]
        public async Task<IActionResult> GetAllLocationMaster()
        {
            _responseDto = await _locationMasterService.GetAllLocationMaster();
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }
        #endregion Location Master

        #region Department Master
        [HttpGet, Route("GetAllDepartmentMaster")]
        public async Task<IActionResult> GetAllDepartmentMaster()
        {
            _responseDto = await _DepartmentMasterService.GetAllDepartmentMaster();
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }
        #endregion Department Master

        #region Plan Master
        [HttpGet, Route("GetAllPlanMasters")]
        public async Task<IActionResult> GetAllPlanMasters()
        {
            _responseDto = await _planMasterService.GetAllPlanMastersAsync();
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpGet, Route("GetAllPlanMastersToCreateSubs")]
        public async Task<IActionResult> GetAllPlanMastersToCreateSubs()
        {
            _responseDto = await _planMasterService.GetPlanMastersToCreateSubs();
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpPost, Route("CreatePlanMaster")]
        public async Task<IActionResult> CreatePlanMaster([FromBody] PlanMasterDto plan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _responseDto = await _planMasterService.AddPlanMasterAsync(plan);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.Created : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpDelete, Route("DeletePlanMaster")]
        public async Task<ActionResult> DeletePlanMaster(int planId)
        {
            await _planMasterService.DeletePlanMasterAsync(planId);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);

        }

        [HttpGet, Route("GetPlanMasterById")]
        public async Task<ActionResult> GetPlanMasterById(int planId)
        {
            await _planMasterService.GetPlanMasterByIdAsync(planId);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);

        }

        [HttpPut, Route("UpdatePlanMaster")]
        public async Task<ActionResult> UpdatePlanMaster([FromBody] PlanMasterDto plan)
        {
            await _planMasterService.UpdatePlanMasterAsync(plan);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);

        }
        #endregion


        #region subplan master
        [HttpPost, Route("CreateSubPlanMaster")]
        public async Task<IActionResult> CreateSubPlanMaster([FromBody] SubPlansMasterRequestDto plan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _responseDto = await _planMasterService.AddSubPlanMasterAsync(plan);
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }

        [HttpPut, Route("UpdateSubPlanMaster")]
        public async Task<IActionResult> UpdateSubPlanMaster([FromBody] SubPlansMasterRequestDto plan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _responseDto = await _planMasterService.UpdateSubPlanMaster(plan);
            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }
        #endregion


        #region Splecial Plan Master
        [HttpGet, Route("GetAllSplPlanMasters")]
        public async Task<IActionResult> GetAllSplPlanMasters()
        {
            _responseDto = await _planMasterService.GetAllSplPlanMastersAsync();
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpGet, Route("GetAllSplPlanMastersToCreateSubs")]
        public async Task<IActionResult> GetAllSplPlanMastersToCreateSubs()
        {
            _responseDto = await _planMasterService.GetSplPlanMastersToCreateInvestment();
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpPost, Route("CreateSplPlanMaster")]
        public async Task<IActionResult> CreateSplPlanMaster([FromBody] SplPlanMasterDto plan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _responseDto = await _planMasterService.AddSplPlanMasterAsync(plan);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.Created : (int)HttpStatusCode.InternalServerError, _responseDto);
        }

        [HttpDelete, Route("DeleteSplPlanMaster")]
        public async Task<ActionResult> DeleteSplPlanMaster(int planId)
        {
            await _planMasterService.DeleteSplPlanMasterAsync(planId);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);

        }

        [HttpGet, Route("GetSplPlanMasterById")]
        public async Task<ActionResult> GetSplPlanMasterById(int planId)
        {
            await _planMasterService.GetSplPlanMasterByIdAsync(planId);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);

        }

        [HttpPut, Route("UpdateSplPlanMaster")]
        public async Task<ActionResult> UpdateSplPlanMaster([FromBody] SplPlanMasterDto plan)
        {
            await _planMasterService.UpdateSplPlanMasterAsync(plan);
            return StatusCode(_responseDto.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, _responseDto);

        }
        #endregion

    }
}
