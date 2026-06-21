using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
	[Table("PLAN_MASTER")]
	public class PlanMaster
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int PlanId { get; set; }

		[Column("PLAN_CODE", TypeName = "varchar(30)")]
		public string PlanCode { get; set; }

		[Column("PLAN_NAME", TypeName = "varchar(50)")]
		public string PlanName { get; set; }

		[Column("DESCRIPTION", TypeName = "varchar(100)")]
		public string Description { get; set; }

		//[Column("PAYOUT_FREQUENCY")]
		//public int PayoutFrequency { get; set; }

		//[Column("INTEREST_RATE", TypeName = "decimal(5,2)")]
		//public decimal InterestRate { get; set; }

		//[Column("MIN_INVESTMENT", TypeName = "decimal(18,2)")]
		//public decimal MinInvestment { get; set; }

		[Column("CREATED_DATE", TypeName = "datetime")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CreatedDate { get; set; }

		[Column("STATUS")]
		public bool Status { get; set; }
		public DateTime? EndDate { get; set; } = null;
    }
}
