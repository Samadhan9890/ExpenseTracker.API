using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.ServiceModels;

namespace ExpenseTracker.Services.Contracts.IContracts
{
	public interface IMenuServices
	{
		public AccessibleMenu GetAccesibleMenus(string userId);

		Task<AccessibleMenu> GetAllAvailableMenus();

		Task<IList<SP_MENUROLE_ACCESS_Result>> GetAccesibleMenusByRoleId(string roleId);

		Task<IList<MenuAccess>> AddUpdateMenuAccess(int roleId, string roleMenuAccess);
	}
}
