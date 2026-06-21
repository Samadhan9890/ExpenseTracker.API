using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Services.Models.EntityModels
{
	[Table("SUB_PLANS_MASTER")]
	public class SubPlansMaster
	{
		[Column("SUB_PLANS_ID", TypeName = "int")]
		public int SubPlansId { get; set; }

        public int PlanId { get; set; }

		[Column("PLAN_CODE", TypeName = "varchar(30)")]
		public string PlanCode { get; set; }

		[Column("PAYOUT_FREQUENCY_IN_MONTHS")]
		public int PayoutFrequencyInMonths { get; set; }

		[Column("INTEREST_RATE", TypeName = "decimal(5,2)")]
		public decimal InterestRate { get; set; }

		[Column("MIN_INVESTMENT", TypeName = "decimal(18,2)")]
		public decimal MinInvestment { get; set; }

		[Column("CREATED_DATE", TypeName = "datetime")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CreatedDate { get; set; }
			
	}
}
