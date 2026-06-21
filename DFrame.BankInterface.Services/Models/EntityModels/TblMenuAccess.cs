using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
	[Table("TBL_MENU_ACCESS", Schema = "dbo")]
	public class TblMenuAccess
	{

		[Key]
		[Column("MENU_ACCESS_ID", TypeName = "INT")]
		public int Id { get; set; }

		[Column("MENU_ID", TypeName = "INT")]
		public int MenuId { get; set; }

		[Column("ROLE_ID", TypeName = "INT")]
		public int RoleId { get; set; }

		[Column("ENTRY_ID", TypeName = "INT")]
		public int? EntryId { get; set; }

		[Column("MODIFY_ID", TypeName = "INT")]
		public int? ModifyId { get; set; }

		[Column("ENTRY_DATE", TypeName = "DATETIME")]
		public DateTime? EntryDate { get; set; }

		[Column("MODIFY_DATE", TypeName = "DATETIME")]
		public DateTime? ModifyDate { get; set; }

		[Column("DELETE_ID", TypeName = "int")]
		public int? DeleteId { get; set; }

		[Column("DELETE_DATE", TypeName = "DATETIME")]
		public DateTime? DeleteDate { get; set; }

		[Column("STATUS", TypeName = "BIT")]
		public bool? Status { get; set; }
	}
}
