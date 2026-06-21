namespace ExpenseTracker.Services.Models.DTOs
{
	public class UpdatePaymentStatusRequestDto
	{
		public string[] Payments { get; set; }
		public int StatusToUpdate { get; set; }
        public string Comments { get; set; }
    }

    public class UpdateInvestmentPaymentStatusRequestDto
    {
        public string[] Payments { get; set; }
        public int StatusToUpdate { get; set; }
        public string Comments { get; set; }
    }
}
