using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.DTOs
{
	public class SubPlansMasterRequestDto
	{
		public int SubPlansId { get; set; }
		[Required]
		public int PlanId { get; set; }
		[Required]
		public string PlanCode { get; set; }
		[Required]
		public int PayoutFrequencyInMonths { get; set; }
		[Required]
		public decimal InterestRate { get; set; }
		[Required]
		public decimal MinInvestment { get; set; }		
	}
}
