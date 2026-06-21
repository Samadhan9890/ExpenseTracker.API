using ExpenseTracker.Services.Models.DTOs;

namespace ExpenseTracker.Services.Services.IServices
{
	public interface IPlanMasterService
	{
		Task<ResponseDto> GetAllPlanMastersAsync();
		Task<ResponseDto> GetPlanMasterByIdAsync(int id);
		Task<ResponseDto> AddPlanMasterAsync(PlanMasterDto plan);
		Task<ResponseDto> UpdatePlanMasterAsync(PlanMasterDto plan);
		Task<ResponseDto> DeletePlanMasterAsync(int id);

		Task<ResponseDto> AddSubPlanMasterAsync(SubPlansMasterRequestDto subPlan);
		Task<ResponseDto> UpdateSubPlanMaster(SubPlansMasterRequestDto subPlan);
		Task<ResponseDto> GetPlanMastersToCreateSubs();


        #region spl plan master
        Task<ResponseDto> GetAllSplPlanMastersAsync();
        Task<ResponseDto> GetSplPlanMasterByIdAsync(int id);
        Task<ResponseDto> AddSplPlanMasterAsync(SplPlanMasterDto plan);
        Task<ResponseDto> UpdateSplPlanMasterAsync(SplPlanMasterDto plan);
        Task<ResponseDto> DeleteSplPlanMasterAsync(int id);

		Task<ResponseDto> GetSplPlanMastersToCreateInvestment();
        #endregion

    }
}
