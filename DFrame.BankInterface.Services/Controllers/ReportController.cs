using ExpenseTracker.Services.Contracts.IContracts.BusinessUnitContracts;
using ExpenseTracker.Services.Contracts.IContracts;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Services.Contracts;
using ExpenseTracker.Services.Services;
using System.Net;
using Azure;
using ExpenseTracker.Services.Utilities.ConstantHelper;
using ExpenseTracker.Services.Utilities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.Services.Models.ServiceModels;
using ExpenseTracker.Services.Contracts.RequestResponseDto;

namespace ExpenseTracker.Services.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly ILogger<MasterController> _logger;
        private ResponseDto _responseDto;
        private readonly IReportMasterService _reportMasterService;

        public ReportController(ILogger<MasterController> logger,
                        ResponseDto responseDto,
                        IReportMasterService reportMasterService)
        {
            _logger = logger;
            _responseDto = responseDto;
            _reportMasterService = reportMasterService;
        }

        #region Report For Admin

        [HttpGet("GetReportManagerList")]
        public async Task<IActionResult> GetReportManagerList()
        {
            try
            {
                _responseDto = await _reportMasterService.GetReportManagerList();
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While getting report manager list. {ex}");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.OK, _responseDto);
            }
        }

        [HttpGet("GetReportManagerDataById")]
        public async Task<IActionResult> GetReportManagerDataById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new Exception("Invalid id");
                }

                _responseDto = await _reportMasterService.GetReportManagerDataById(id);
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While getting report data. {ex}");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.OK, _responseDto);
            }
        }

        [HttpPost("AddUpdateReport")]
        public async Task<IActionResult> AddUpdateReport([FromBody] CustomReportMasterDto reportDto)
        {
            try
            {
                var userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);
                _responseDto = await _reportMasterService.AddUpdateReport(reportDto, userDetails);
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While add and update report data. {ex}");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.OK, _responseDto);
            }
        }

        [HttpDelete, Route("DeleteReportMaster")]
        public async Task<IActionResult> DeleteReportMaster(int id)
        {
            _responseDto = await _reportMasterService.DeleteReportMaster(id);
            return Ok(_responseDto);
        }

        #endregion Report For Admin


        #region Report For User
        [HttpGet("GetReportViewerList")]
        public async Task<IActionResult> GetReportViewerList()
        {
            try
            {
                var userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);
                _responseDto = await _reportMasterService.GetReportViewerList(userDetails);
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While getting report master list. {ex}");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.OK, _responseDto);
            }
        }

        [HttpGet("GetReportViewerDataById")]
        public async Task<IActionResult> GetReportViewerDataById(int id)
        {
            try
            {
                var userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);
                _responseDto = await _reportMasterService.GetReportViewerDataById(id, userDetails);
                return Ok(_responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While getting report viewer data. {ex}");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.OK, _responseDto);
            }
        }

        private void ClearOldReports(string outputPath)
        {
            string path = Path.GetDirectoryName(outputPath);
            try
            {
                if (Directory.Exists(path))
                {
                    _logger.LogInformation($"Path exists - {path}");
                    var files = Directory.GetFiles(path);
                    var oldFiles = files.Where(f => System.IO.File.GetCreationTime(f) < DateTime.Now.AddMinutes(-10));

                    foreach (string oldfile in oldFiles)
                    {
                        System.IO.File.Delete(oldfile);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to delete old files from path - {path} - {ex}");
            }
        }

        [HttpPost("GenerateExcelForReport")]
        public async Task<IActionResult> GenerateExcelForReport(int id, [FromBody] ReportViewerFilter reportView)
        {
            try
            {
				UserClaims userDetails = UserClaimsHelper.GetClaims(HttpContext.User.Identity as ClaimsIdentity);
                _responseDto = await _reportMasterService.GenerateExcelForReport(id, reportView, userDetails);
                
                ReportExcelModel report = _responseDto.Result != null ? (ReportExcelModel)_responseDto.Result : throw new Exception("no data found") ;
                         
                string reportPath = $"{report.ReportPath}{report.ReportName}_{DateTime.Now.ToString("ddMMyyyy_hh_mm")}.xlsx";

                string npoiTempPath = report.SystemTempPath;
                string npoiPath = report.ReportPath;

                try
                {
                    //Ceate temp directory if not exist
                    if (!Directory.Exists(npoiTempPath)) { Directory.CreateDirectory(npoiTempPath); }
                    if (!Directory.Exists(npoiPath)) { Directory.CreateDirectory(npoiPath); }
                    
                    ClearOldReports(reportPath);                    
                    ClearOldReports(npoiTempPath);                    
                    
                    ExcelGenerateHelper.SaveExcelToPath(reportPath, report.Data);

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured while saving the report - {ex}");
                }
                
				return File(System.IO.File.ReadAllBytes(reportPath), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(reportPath));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While getting report file. {ex}");
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.OK, _responseDto);
            }

        }

        #endregion Report For User

    }
}
