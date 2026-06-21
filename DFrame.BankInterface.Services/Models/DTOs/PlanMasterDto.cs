using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.DTOs
{
	public class PlanMasterDto
	{
		public int PlanId { get; set; }
		[Required]
		public string PlanCode { get; set; }
		[Required]
		public string PlanName { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		//public int PayoutFrequency { get; set; }
		//[Required]
		//public decimal InterestRate { get; set; }
		//[Required]
		//public decimal MinInvestment { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool Status { get; set; }
        public List<SubPlansMasterResponseDto>? SubPlans { get; set; }
    }
}
