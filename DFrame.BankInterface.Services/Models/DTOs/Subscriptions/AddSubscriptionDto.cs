using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.DTOs.Subscriptions
{
	public class AddSubscriptionDto
	{
		public int SubscriptionId { get; set; }
		[Required(ErrorMessage ="The subscription type is mandatory.")]
		public string SubscriptionType { get; set; }
		public int? OldSubscriptionId { get; set; }
		[Required(ErrorMessage = "The ClientId is mandatory.")]
		public int ClientId { get; set; }
		[Required(ErrorMessage = "The Date of investment is mandatory.")]
		public DateTime DateOfInvestment { get; set; }
		[Required(ErrorMessage = "The plan code is mandatory.")]
		public string PlanCode { get; set; }
		[Required(ErrorMessage = "The plan name is mandatory.")]
		public string PlanName { get; set; }
		[Range(double.Epsilon, double.MaxValue,ErrorMessage ="The amount should be greater than 0")]
		public decimal InvestmentAmount { get; set; }
		[Range(1, 12, ErrorMessage = "The payout frequency shoulde be between 1 and 12")]
		public int PayoutFrequency { get; set; }
		[Range(1, 100, ErrorMessage = "The periodic payble interset should be between 1 and 100")]
		public decimal PayoutFrequencyInterestRate { get; set; }
		
		//to be handled in code
		public DateTime MaturityDate { get; set; }		
		
		public decimal TotalInterest { get; set; }
		public string? ApprovedBy { get; set; }
		public DateTime? ApprovedDate { get; set; }
		public string? BorrowLetterStatus { get; set; }

		public string? PayoutMethod { get; set; }
		public string? PayoutBankName { get; set; }
		public string? PayoutBankAccountNo { get; set; }
		public string? PayoutBankIfscCode { get; set; }
		public string? PayoutBankAccountHolderName { get; set; }
		public string? UpiId { get; set; }
		public string? Notes { get; set; }
		public int? Status { get; set; }				
		public bool? IsPaymentScheduleAvailable { get; set; }
		public string? InvestmentReceivedDetails { get; set; }
		public DateTime? InvestmentReceivedDate { get; set; }
		public string? ReceivedInvestmentMethod { get; set; }

    }
}
