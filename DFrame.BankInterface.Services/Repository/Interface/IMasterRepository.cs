using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Repository.Interface
{
	public interface IMasterRepository
	{
		#region bank Master
		Task<IList<TblBankMaster>> GetAllBankMastersAsync();
		Task<TblBankMaster> GetBankMasterByIdAsync(int bankId);
		Task<TblBankMaster> AddBankMasterAsync(TblBankMaster bank);
		Task DeleteBankMasterAsync(int bankId);
		Task UpdateBankMaster(BankMasterDto bank);		
        #endregion

        #region Role Master
        Task<IList<TblRole>> GetAllRoles();
        Task<TblRole> GetRoleById(int roleId);
        Task<TblRole> AddRole(TblRole role);
        Task<TblRole> UpdateRole(TblRole role);
        Task DeleteRole(int roleId);
        #endregion

        #region Location Master
        Task<IList<TblLocation>> GetAllLocations();
        #endregion

        #region Department Master
        Task<IList<TblDepartment>> GetAllDepartments();
		#endregion

		#region Plan Master
		Task<IList<PlanMaster>> GetAllPlanMastersAsync();
		Task<PlanMaster> GetPlanMasterByIdAsync(int planId);
		Task<PlanMaster> AddPlanMasterAsync(PlanMaster plan);
		Task DeletePlanMasterAsync(int planId);
		Task UpdatePlanMaster(PlanMasterDto plan);
        #endregion

        #region Special plan Master
        Task<IList<SplPlanMaster>> GetAllSplPlanMastersAsync();
        Task<SplPlanMaster> GetSplPlanMasterByIdAsync(int planId);
        Task<SplPlanMaster> AddSplPlanMasterAsync(SplPlanMaster plan);
        Task DeleteSplPlanMasterAsync(int planId);
        Task UpdateSplPlanMaster(SplPlanMasterDto plan);
        #endregion

        #region Sub Plans master
        Task<IList<SubPlansMasterResponseDto>> GetAllSubPlanMastersByPlanIdAsync(int planId);
		Task AddSubPlanMasterAsync(SubPlansMasterRequestDto subPlan);		
		Task UpdateSubPlanMaster(SubPlansMasterRequestDto subPlan);
		#endregion
	}
}
