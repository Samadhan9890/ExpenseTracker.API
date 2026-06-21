namespace ExpenseTracker.Services.Models
{
	public class UpdateSubscriptionStatusRequestDto
	{
        public Guid Guid { get; set; }
        public int  SubscriptionId { get; set; }
        public int StatusToUpdate { get; set; }
        public string Comments { get; set; }
        public string? ActionBy { get; set; }
        public int? OldSubscriptionId { get; set; }
    }
}
