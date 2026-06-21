using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Services.Models.DTOs
{
	public class SubPlansMasterResponseDto
	{		
		public int SubPlansId { get; set; }
		public int PlanId { get; set; }	
		public string PlanCode { get; set; }
		public int PayoutFrequencyInMonths { get; set; }
		public decimal InterestRate { get; set; }
		public decimal MinInvestment { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
