using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Repository.Interface
{
    public interface IMenuRepository
    {
        public List<TblMenu> GetAccessibleMenus(string userId);

        Task<IList<TblMenu>> GetAllAvailableMenus();

        Task<IList<TblMenu>> GetAccesibleMenusByRoleId(string roleId);

        Task<IList<TblMenuAccess>> InsertMenuAccessEntries(int roleId, string roleMenuAccess);
    }
}
