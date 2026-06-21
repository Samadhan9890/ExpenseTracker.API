namespace ExpenseTracker.Services.Models.DTOs.Subscriptions
{
	public class BorrowLetterDetailsResponseDto
	{
		public int SubscriptionId { get; set; }
		public Guid Guid { get; set; }

        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }
        public string TrackingNo { get; set; }
        public string SentDate { get; set; }
        public int Status { get; set; }
    }
}
