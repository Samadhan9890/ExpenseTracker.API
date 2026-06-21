using AutoMapper;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;

namespace ExpenseTracker.Services.Services
{
	public class PlanMasterService : IPlanMasterService
	{
		private readonly IMasterRepository _masterRepository;
		private ResponseDto _responseDto;
		private readonly ILogger<PlanMasterService> _logger;
		private readonly IMapper _mapper;

		public PlanMasterService(IMasterRepository masterRepository, ResponseDto responseDto, ILogger<PlanMasterService> logger, IMapper mapper)
		{
			_masterRepository = masterRepository;
			_responseDto = responseDto;
			_logger = logger;
			_mapper = mapper;
		}

		public async Task<ResponseDto> AddPlanMasterAsync(PlanMasterDto plan)
		{
			try
			{
				var planMaster = _mapper.Map<PlanMaster>(plan);
				await _masterRepository.AddPlanMasterAsync(planMaster);				
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = $"Unable to add plan master. {ex.Message}";
			}

			return _responseDto;
		}

		public async Task<ResponseDto> DeletePlanMasterAsync(int id)
		{
			try
			{
				await _masterRepository.DeletePlanMasterAsync(id);
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
				_logger.LogError($"Unable to delete plan master - {ex.Message} ");
			}
			return _responseDto;
		}

		public async Task<ResponseDto> GetAllPlanMastersAsync()
		{
			try
			{
				var plans = await _masterRepository.GetAllPlanMastersAsync();
				List<PlanMasterDto> planMasterDTOs = _mapper.Map<List<PlanMasterDto>>(plans);	
				foreach(var plan in planMasterDTOs)
				{
					var subPlans = await _masterRepository.GetAllSubPlanMastersByPlanIdAsync(plan.PlanId);
					plan.SubPlans = new();
					plan.SubPlans.AddRange(subPlans);
				}
				_responseDto.Result = planMasterDTOs;
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = "Unable to get Plan Masters";
				_logger.LogError($"Unable to get the plan master - {ex}");

			}
			return _responseDto;
		}

		public async Task<ResponseDto> GetPlanMasterByIdAsync(int id)
		{
			try
			{
				var planMaster = await _masterRepository.GetPlanMasterByIdAsync(id) ?? throw new Exception($"Plan Master not found for ID {id}");
				var plan = _mapper.Map<PlanMasterDto>(planMaster);
				IList<SubPlansMasterResponseDto> subplans = await _masterRepository.GetAllSubPlanMastersByPlanIdAsync(planMaster.PlanId);
				plan.SubPlans = new List<SubPlansMasterResponseDto>();
				plan.SubPlans.AddRange(subplans);				
				_responseDto.Result = plan;
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
				_logger.LogError($"Unable to get the plan master - {ex}");
			}

			return _responseDto;

		}

		public async Task<ResponseDto> UpdatePlanMasterAsync(PlanMasterDto plan)
		{
			try
			{
				await _masterRepository.UpdatePlanMaster(plan);
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
			}

			return _responseDto;
		}

		public async Task<ResponseDto> AddSubPlanMasterAsync(SubPlansMasterRequestDto subPlan)
		{
			try
			{
				await _masterRepository.AddSubPlanMasterAsync(subPlan);
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
			}
			return _responseDto;

		}

		public async Task<ResponseDto> UpdateSubPlanMaster(SubPlansMasterRequestDto subPlan)
		{
			try
			{
				await _masterRepository.UpdateSubPlanMaster(subPlan);
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
			}
			return _responseDto;
		}

		public async Task<ResponseDto> GetPlanMastersToCreateSubs()
		{
			try
			{
				var plans = await _masterRepository.GetAllPlanMastersAsync();
				List<PlanMasterDto> planMasterDTOs = _mapper.Map<List<PlanMasterDto>>(plans);
				List<SubPlansMasterResponseDto> subplansResponse = new List<SubPlansMasterResponseDto>();
				foreach (var plan in planMasterDTOs.Where(p=> p.Status == true))
				{
					var subPlans = await _masterRepository.GetAllSubPlanMastersByPlanIdAsync(plan.PlanId);
					subplansResponse.AddRange(subPlans);
				}
				_responseDto.Result = subplansResponse;
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = "Unable to get Plan Masters";
				_logger.LogError($"Unable to get the plan master - {ex}");

			}
			return _responseDto;
		}

        public async Task<ResponseDto> GetAllSplPlanMastersAsync()
        {
            try
            {
                var plans = await _masterRepository.GetAllSplPlanMastersAsync();
                List<SplPlanMasterDto> planMasterDTOs = _mapper.Map<List<SplPlanMasterDto>>(plans);               
                _responseDto.Result = planMasterDTOs;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Unable to get Plan Masters";
                _logger.LogError($"Unable to get the plan master - {ex}");

            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetSplPlanMasterByIdAsync(int id)
        {
            try
            {
                var planMaster = await _masterRepository.GetSplPlanMasterByIdAsync(id) ?? throw new Exception($"Plan Master not found for ID {id}");
                var plan = _mapper.Map<SplPlanMasterDto>(planMaster);              
                _responseDto.Result = plan;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError($"Unable to get the plan master - {ex}");
            }

            return _responseDto;
        }

        public async Task<ResponseDto> AddSplPlanMasterAsync(SplPlanMasterDto plan)
        {
            try
            {
                var planMaster = _mapper.Map<SplPlanMaster>(plan);
                await _masterRepository.AddSplPlanMasterAsync(planMaster);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to add plan master. {ex.Message}";
            }

            return _responseDto;
        }

        public async Task<ResponseDto> UpdateSplPlanMasterAsync(SplPlanMasterDto plan)
        {
            try
            {
                await _masterRepository.UpdateSplPlanMaster(plan);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        public async Task<ResponseDto> DeleteSplPlanMasterAsync(int id)
        {
            try
            {
                await _masterRepository.DeleteSplPlanMasterAsync(id);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError($"Unable to delete plan master - {ex.Message} ");
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetSplPlanMastersToCreateInvestment()
        {
            try
            {
                var plans =  _masterRepository.GetAllSplPlanMastersAsync().GetAwaiter().GetResult().Where(p=> p.Status == true);
                List<SplPlanMasterDto> planMasterDTOs = _mapper.Map<List<SplPlanMasterDto>>(plans);               
                _responseDto.Result = planMasterDTOs;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Unable to get Plan Masters";
                _logger.LogError($"Unable to get the plan master - {ex}");

            }
            return _responseDto;
        }
    }
}
