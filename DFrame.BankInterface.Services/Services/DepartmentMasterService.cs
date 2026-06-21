using AutoMapper;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;

namespace ExpenseTracker.Services.Services
{
    public class DepartmentMasterService:IDepartmentMasterService
    {
        private readonly IMasterRepository _masterRepository;
        private ResponseDto _responseDto;
        private readonly ILogger<DepartmentMasterService> _logger;
        private readonly IMapper _mapper;
        public DepartmentMasterService(IMasterRepository masterRepository, ResponseDto responseDto, ILogger<DepartmentMasterService> logger, IMapper mapper)
        {
            _masterRepository = masterRepository;
            _responseDto = responseDto;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetAllDepartmentMaster()
        {
            _logger.LogInformation($"Get department list");
            try
            {
                var department = await _masterRepository.GetAllDepartments();
                List<DepartmentDto> departmentDto = _mapper.Map<List<DepartmentDto>>(department);
                _responseDto.Result = departmentDto;
                _logger.LogInformation($"Retunrining department list");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Unable to get department list";
            }
            return _responseDto;
        }
    }
}
