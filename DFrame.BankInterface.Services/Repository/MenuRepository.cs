using ExpenseTracker.Services.Contracts.IContracts.JWTContracts;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services.Repository
{
    public class MenuRepository : IMenuRepository
	{
        private readonly AppDBContext _context;

        public MenuRepository(AppDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all the menus which are accessible to current user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TblMenu> GetAccessibleMenus(string userId)
        {
            //Dictionary<string, string> params = new Dictionary<string, string>();

            Dictionary<string, string> spParams = new Dictionary<string, string>();
            spParams.Add("@CMDTYPE", "GETMENUDETAILS");
            spParams.Add("@INTERNAL_USER_ID", userId);

            SqlAccess sqlAccess = new SqlAccess(_context);
            var dtResult = sqlAccess.ExecuteScalarSp("USP_MENU_ACCESS", spParams);

            List<TblMenu> menus = DatatableHelper.BindList<TblMenu>(dtResult);

            return menus;
        }

        public async Task<IList<TblMenu>> GetAllAvailableMenus()
        {
            var menus = await _context.Menus.ToListAsync();
            return menus;
        }

        public async Task<IList<TblMenu>> GetAccesibleMenusByRoleId(string roleId)
        {
            int roleIdInt = int.Parse(roleId);

            var query = from ma in _context.MenuAccess
                        join tm in _context.Menus
                            on ma.MenuId equals tm.MENU_ID
                        where ma.RoleId == roleIdInt && ma.Status == true
                        orderby tm.MENU_LEVEL, tm.ORDER_LEVEL
                        select tm;

            return await query.Distinct().ToListAsync();
        }

        public async Task<IList<TblMenuAccess>> InsertMenuAccessEntries(int roleId, string? roleMenuAccess)
        {
            bool hasExistingEntries = await _context.MenuAccess.AnyAsync(ma => ma.RoleId == roleId);

            if (hasExistingEntries)
            {
                var entriesToDelete = _context.MenuAccess
                    .Where(ma => ma.RoleId == roleId)
                    .ToList();

                _context.MenuAccess.RemoveRange(entriesToDelete);
            }


            if (string.IsNullOrEmpty(roleMenuAccess))
            {
                await _context.SaveChangesAsync();
                return new List<TblMenuAccess>();
            }
            else
            {
                var menuIds = roleMenuAccess.Split(',')
                    .Select(int.Parse)
                    .ToList();

                List<TblMenuAccess> newEntries = new List<TblMenuAccess>();

                foreach (var menuId in menuIds)
                {
                    var newEntry = new TblMenuAccess
                    {
                        RoleId = roleId,
                        MenuId = menuId,
                        //EntryId = userId,
                        EntryDate = DateTime.Now,
                        Status = true
                    };

                    newEntries.Add(newEntry);
                }

                _context.MenuAccess.AddRange(newEntries);
                await _context.SaveChangesAsync();
                return newEntries;
            }
        }
    }

}
