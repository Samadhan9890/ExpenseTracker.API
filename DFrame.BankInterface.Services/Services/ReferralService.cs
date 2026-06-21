using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;

namespace ExpenseTracker.Services.Services
{
    public class ReferralService : IReferralService
    {
        private readonly IReferralsRepository _refRepository;
        private ResponseDto _responseDto;
        public ReferralService(IReferralsRepository referralsRepository)
        {
            _refRepository = referralsRepository;
            _responseDto = new ResponseDto();
        }
        public async Task<ResponseDto> GetAllBdPerformance()
        {
            try
            {
                _responseDto.Result = await _refRepository.GetAllBdPerformance();
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;

        }

        public async Task<ResponseDto> GetBdHierarchybyClientId(int clientId)
        {
            try
            {
                _responseDto.Result = await _refRepository.GetAllBusinessDevTeamHierarchyByClientId(clientId);
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
