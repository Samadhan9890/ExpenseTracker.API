using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
	[Table("TBL_STATUS")]
	public class TblStatus
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("ID")]
		public int Id { get; set; }

		[Column("PROCESS_ID")]
		public int? ProcessId { get; set; }

		[Column("FORM_ID")]
		public int? FormId { get; set; }

		[Column("STATUS")]
		public int? StatusValue { get; set; }

		[Column("DESCRIPTION", TypeName = "varchar(500)")]
		public string Description { get; set; }

		[Column("NEXT_STATUS")]
		public int? NextStatus { get; set; }

		[Column("PREVIOUS_STATUS")]
		public int? PreviousStatus { get; set; }
	}

}
