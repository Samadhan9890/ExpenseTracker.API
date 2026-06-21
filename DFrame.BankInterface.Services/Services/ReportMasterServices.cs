using AutoMapper;
using ExpenseTracker.Services.Contracts;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Models.ServiceModels;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;

namespace ExpenseTracker.Services.Services
{
    public class ReportMasterServices: IReportMasterService
    {
        private readonly IReportMasterRepository _reportMasterRepository;
        private ResponseDto _responseDto;
        private readonly ILogger<BankMasterServices> _logger;
        private readonly IMapper _mapper;
        public ReportMasterServices(IReportMasterRepository reportMasterRepository, ResponseDto responseDto, ILogger<BankMasterServices> logger, IMapper mapper)
        {
            _reportMasterRepository = reportMasterRepository;
            _responseDto = responseDto;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetReportManagerList()
        {
            _logger.LogInformation($"Get Report Manager list");
            try
            {
                var reports = await _reportMasterRepository.GetReportManagerList();
                List<CustomReportMasterDto> reportsDto = _mapper.Map<List<CustomReportMasterDto>>(reports);
                _responseDto.Result = reportsDto;
                _responseDto.IsSuccess = true;
                _responseDto.Message = "SUCCESS";
                _logger.LogInformation($"Retunrining Report Manager list");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Unable to get Report Manager list";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetReportManagerDataById(int id)
        {
            _logger.LogInformation($"Get Report Manager for ID {id}");
            try
            {
                var reports = await _reportMasterRepository.GetReportManagerDataById(id);
                CustomReportMasterDto reportsDto = _mapper.Map<CustomReportMasterDto>(reports);
                _responseDto.Result = reportsDto;
                _responseDto.IsSuccess = true;
                _responseDto.Message = "SUCCESS";
                _logger.LogInformation($"Retunrining Report Manager Details for ID {id}");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to get Report Manager error - {ex.Message}";
            }

            return _responseDto;
        }

        public async Task<ResponseDto> AddUpdateReport(CustomReportMasterDto reportData, UserClaims userDetails)
        {
            try
            {
                _logger.LogInformation($"Report Manager Creation/update Started.");
                TblCustomReportMaster tblReport = _mapper.Map<TblCustomReportMaster>(reportData);
                TblUser tblUser = _mapper.Map<TblUser>(userDetails);
                tblReport = await _reportMasterRepository.AddUpdateReport(tblReport, tblUser);
                CustomReportMasterDto report = _mapper.Map<CustomReportMasterDto>(tblReport);
                _responseDto.Result = report;
                _responseDto.IsSuccess = true;
                _responseDto.Message = "SUCCESS";
                _logger.LogInformation($"Report Manager add/update Successfully.");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to add/update Report Manager error - {ex.Message}";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> DeleteReportMaster(int id)
        {
            try
            {
                _logger.LogInformation($"Delete Report Manager with id {id}");
                await _reportMasterRepository.DeleteReportMaster(id);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "SUCCESS";
                _logger.LogInformation($"Report Manager deleted Successfully.");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to delete Report Manager id {id} error - {ex.Message}";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetReportViewerList(UserClaims userDetails)
        {
            _logger.LogInformation($"Get Report viewer list");
            try
            {
                TblUser tblUser = _mapper.Map<TblUser>(userDetails);
                var reports = await _reportMasterRepository.GetReportViewerList(tblUser);
                List<CustomReportMasterDto> reportsDto = _mapper.Map<List<CustomReportMasterDto>>(reports);
                _responseDto.Result = reportsDto;
                _responseDto.IsSuccess = true;
                _responseDto.Message = "SUCCESS";
                _logger.LogInformation($"Retunrining Report viewer list");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Unable to get Report viewer list";
            }
            return _responseDto;
        }


        public async Task<ResponseDto> GetReportViewerDataById(int id, UserClaims userDetails)
        {
            _logger.LogInformation($"Get Report viewer for ID {id}");
            try
            {
                TblUser tblUser = _mapper.Map<TblUser>(userDetails);
                var report = await _reportMasterRepository.GetReportViewerDataById(id, tblUser);
                CustomReportMasterDto reportsDto = _mapper.Map<CustomReportMasterDto>(report);
                _responseDto.Result = reportsDto;
                _responseDto.IsSuccess = true;
                _responseDto.Message = "SUCCESS";
                _logger.LogInformation($"Retunrining Report viewer Details for ID {id}");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to get Report viewer error - {ex.Message}";
            }

            return _responseDto;
        }

        public async Task<ResponseDto> GenerateExcelForReport(int id, ReportViewerFilter reportView, UserClaims userDetails)
        {
             _logger.LogInformation($"Generate Report excel for ID {id}");
            try
            {
                TblUser tblUser = _mapper.Map<TblUser>(userDetails);
                _responseDto.Result = await _reportMasterRepository.GenerateExcelForReport(id, reportView, tblUser);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "SUCCESS";
                _logger.LogInformation($"Retunrining Report excel for ID {id}");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to Generate Report excel error - {ex.Message}";
            }

            return _responseDto;
        }
    }
}
