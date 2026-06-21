namespace ExpenseTracker.Services.Models.ServiceModels
{
	public class MenuAccess
	{
		public int MenuAccessId { get; set; }
		public int MenuId { get; set; }
		public int RoleId { get; set; }
		public int? EntryId { get; set; }
		public int? ModifyId { get; set; }
		public DateTime? EntryDate { get; set; }
		public DateTime? ModifyDate { get; set; }
		public int? DeleteId { get; set; }
		public DateTime? DeleteDate { get; set; }
		public bool? Status { get; set; }
	}
}
