namespace ExpenseTracker.Services.Models.DTOs
{
	public class UpdateManualPaymentStatusDto
	{
        public Guid Guid { get; set; }
        public int Status { get; set; }
        public string? PaymentUtr { get; set; }
        public string? Comments { get; set; }
        public string? PaidBy { get; set; }

    }

    public class UpdateManualInvestmentPaymentStatusDto
    {
        public Guid Guid { get; set; }
        public int Status { get; set; }
        public string? PaymentUtr { get; set; }
        public string? Comments { get; set; }
        public string? PaidBy { get; set; }
        public string? BankingDetails { get; set; }

    }
}
