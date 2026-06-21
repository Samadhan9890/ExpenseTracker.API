namespace ExpenseTracker.Services.Models.DTOs.Subscriptions
{
	public class BorrowLetterPrintDto
	{
        public string ClientName { get; set; }
        public decimal InvestmentAmount { get; set; }
        public string InvestmentDate { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }

    }
}
