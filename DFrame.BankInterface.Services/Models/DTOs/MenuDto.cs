using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Services.Models.DTOs
{
	public class SubMenu
	{
		public int SubId { get; set; }
		public string? SubName { get; set; }
        public string Link { get; set; }
    }

	public class ParentMenu
	{
		public int MenuId { get; set; }
		public string? Name { get; set; }
        public string IconComponent { get; set; }
        public List<SubMenu>? SubMenus { get; set; }
	}

	public class AccessibleMenu
	{
		public List<ParentMenu>? MenuList { get; set; }

	}

	public class SP_MENUROLE_ACCESS_Result
	{
		public int MENU_ID { get; set; }
		public string? MENU_DESC { get; set; }
		public string? MENU_URL { get; set; }
		public Nullable<int> PARENT_MENU { get; set; }
		public Nullable<int> MENU_LEVEL { get; set; }
        public Nullable<int> ENTRY_ID { get; set; }
        public Nullable<int> MODIFY_ID { get; set; }
        public DateTime? ENTRY_DATE { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public Nullable<int> DELETE_ID { get; set; }
        public DateTime? DELETE_DATE { get; set; }
        public bool? STATUS { get; set; }
        public Nullable<int> ORDER_LEVEL { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
    }
}
