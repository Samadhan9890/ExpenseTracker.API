using AutoMapper;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;

namespace ExpenseTracker.Services.Services
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly IAuditTrailRepository _auditTrailRepository;
        private ResponseDto _responseDto;
        private readonly IMapper _mapper;

        public AuditTrailService(IAuditTrailRepository auditTrailRepository, ResponseDto responseDto, IMapper mapper)
        {
            _auditTrailRepository = auditTrailRepository;
            _responseDto = responseDto;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetAuditTrailAsync(string processIds, int unqId)
        {
            try
            {
                if (processIds == null)
                {
                    throw new Exception("Invalid Process Ids");
                }
                List<TblAuditTrail> auditTrails = await _auditTrailRepository.GetAuditTrailAsync(processIds, unqId);
                _responseDto.Result = _mapper.Map<List<AuditTrailDto>>(auditTrails);
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;

        }

        public async Task<ResponseDto> GetNcAuditTrailAsync(int unqId, string module)
        {
            try
            {
                var audits = await _auditTrailRepository.GetNcAuditTrailAsync(unqId, module);
                _responseDto.Result = audits;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
    }
}

