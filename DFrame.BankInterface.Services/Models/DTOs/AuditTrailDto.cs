namespace ExpenseTracker.Services.Models.DTOs
{
  public class AuditTrailDto
	{
		public int ProcessID { get; set; }
		public string? ActionDescription { get; set; }
		public string? Comments { get; set; }
		public string UserName { get; set; }
		public DateTime ActionDate { get; set; }
	}

}
