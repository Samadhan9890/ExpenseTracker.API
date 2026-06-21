using AutoMapper;
using ExpenseTracker.Services.Contracts.IContracts;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Models.ServiceModels;
using ExpenseTracker.Services.Repository.Interface;

namespace ExpenseTracker.Services.Contracts
{
	public class MenuServices : IMenuServices
	{
		private readonly IMenuRepository _menurepository;
		private readonly IMapper _mapper;
		public MenuServices(IMenuRepository menuRepository, IMapper mapper)
		{

			_menurepository = menuRepository;
			_mapper = mapper;
		}

		public AccessibleMenu GetAccesibleMenus(string userId)
		{
			AccessibleMenu menuAccess = new AccessibleMenu();
			menuAccess.MenuList = new List<ParentMenu>();

			var lstMenus = _menurepository.GetAccessibleMenus(userId);

			if (lstMenus == null || lstMenus.Count == 0)
			{
				throw new Exception("Unable to get autorized menu list.");
			}

			var parentMenus = lstMenus.Where(m => m.PARENT_MENU == null).ToList().OrderBy(x=>x.ORDER_LEVEL);


			foreach (var menu in parentMenus)
			{
				ParentMenu parentMenu = new ParentMenu()
				{
					MenuId = menu.MENU_ID,
					Name = menu.MENU_DESC
				};

				parentMenu.SubMenus = new List<SubMenu>();

				var subMenus = lstMenus.Where(m => m.PARENT_MENU == menu.MENU_ID).ToList();
				foreach (var subMenu in subMenus)
				{
					SubMenu sub = new SubMenu()
					{
						SubId = subMenu.MENU_ID,
						SubName = subMenu.MENU_DESC,
						Link = subMenu.MENU_URL
					};
					parentMenu.SubMenus.Add(sub);
				}

				menuAccess.MenuList.Add(parentMenu);

			}

			return menuAccess;
		}


		public async Task<AccessibleMenu> GetAllAvailableMenus()
		{
			AccessibleMenu menuAccess = new AccessibleMenu();
			menuAccess.MenuList = new List<ParentMenu>();

			var lstMenus = await _menurepository.GetAllAvailableMenus();

			if (lstMenus == null)
			{
				throw new Exception("Unable to get authorized menu list.");
			}

			var parentMenus = lstMenus.Where(m => m.PARENT_MENU == null).ToList();


			foreach (var menu in parentMenus)
			{
				ParentMenu parentMenu = new ParentMenu()
				{
					MenuId = menu.MENU_ID,
					Name = menu.MENU_DESC
				};

				parentMenu.SubMenus = new List<SubMenu>();

				var subMenus = lstMenus.Where(m => m.PARENT_MENU == menu.MENU_ID).ToList();
				foreach (var subMenu in subMenus)
				{
					SubMenu sub = new SubMenu()
					{
						SubId = subMenu.MENU_ID,
						SubName = subMenu.MENU_DESC
					};
					parentMenu.SubMenus.Add(sub);
				}

				menuAccess.MenuList.Add(parentMenu);

			}

			return menuAccess;
		}

		public async Task<IList<SP_MENUROLE_ACCESS_Result>> GetAccesibleMenusByRoleId(string roleId)
		{
			var lstMenus = await _menurepository.GetAccesibleMenusByRoleId(roleId);

			if (lstMenus == null)
			{
				throw new Exception("Unable to get authorized menu list.");
			}

			IList<SP_MENUROLE_ACCESS_Result> menulist = _mapper.Map<IList<SP_MENUROLE_ACCESS_Result>>(lstMenus);
			return menulist;
		}

		public async Task<IList<MenuAccess>> AddUpdateMenuAccess(int roleId, string? roleMenuAccess)
		{
			if (roleId < 0)
			{
				throw new Exception("Unable to update menu access.");
			}

			IList<TblMenuAccess> menus = await _menurepository.InsertMenuAccessEntries(roleId, roleMenuAccess);
			List<MenuAccess> menuAccesses = _mapper.Map<List<MenuAccess>>(menus);
			return menuAccesses;
		}
	}
}
