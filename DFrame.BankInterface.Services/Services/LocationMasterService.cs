using AutoMapper;
using ExpenseTracker.Services.Contracts;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;

namespace ExpenseTracker.Services.Services
{
    public class LocationMasterService : ILocationMasterService
    {

        private readonly IMasterRepository _masterRepository;
        private ResponseDto _responseDto;
        private readonly ILogger<LocationMasterService> _logger;
        private readonly IMapper _mapper;
        public LocationMasterService(IMasterRepository masterRepository, ResponseDto responseDto, ILogger<LocationMasterService> logger, IMapper mapper)
        {
            _masterRepository = masterRepository;
            _responseDto = responseDto;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetAllLocationMaster()
        {
            _logger.LogInformation($"Get location list");
            try
            {
                var location = await _masterRepository.GetAllLocations();
                List<LocationDTO> locationDto = _mapper.Map<List<LocationDTO>>(location);
                _responseDto.Result = locationDto;
                _logger.LogInformation($"Retunrining location list");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Unable to get location list";
            }
            return _responseDto;
        }
    }
}
