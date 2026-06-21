using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.DTOs.Subscriptions
{
	public class ClientWithSubscriptionDto
	{
		public int ClientId { get; set; }		
		public string Name { get; set; }		
		public string AadharNo { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string? CreatedBy { get; set; }

        public List<SubscriptionResponseDto> Subscriptions { get; set; }
    }
}
