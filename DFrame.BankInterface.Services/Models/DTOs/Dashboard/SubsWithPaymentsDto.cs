using ExpenseTracker.Services.Models.DTOs.Subscriptions;

namespace ExpenseTracker.Services.Models.DTOs.Dashboard
{
	public class SubsWithPaymentsDto
	{
		public int SubscriptionId { get; set; }
		public string SubscriptionType { get; set; }
		public int? OldSubscriptionId { get; set; }
		public int ClientId { get; set; }
		public DateTime DateOfInvestment { get; set; }
		public string PlanCode { get; set; }
		public string PlanName { get; set; }
		public decimal InvestmentAmount { get; set; }
		public int PayoutFrequency { get; set; }
		public decimal TotalInterest { get; set; }
		public decimal PayoutFrequencyInterestRate { get; set; }
		public DateTime MaturityDate { get; set; }
		public string ApprovedBy { get; set; }
		public DateTime ApprovedDate { get; set; }
		public int BorrowLetterStatus { get; set; }
		public string PayoutMethod { get; set; }
		public string PayoutBankName { get; set; }
		public string PayoutBankAccountNo { get; set; }
		public string PayoutBankIfscCode { get; set; }
		public string PayoutBankAccountHolderName { get; set; }
		public string UpiId { get; set; }
		public string Notes { get; set; }
		public int Status { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public int Tenure { get; set; }
        public bool isPaymentScheduleAvailable { get; set; }
        public Guid Guid { get; set; }

        public List<PaymentScheduleDto> Payments { get; set; }
        public BorrowLetterDetailsResponseDto BorrowLetterDetails { get; set; }

    }
}
