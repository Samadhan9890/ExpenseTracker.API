namespace ExpenseTracker.Services.Models.DTOs
{
    public class PaymentProofDto
    {
        public int Id { get; set; }
        public Guid PaymentScheduleGuid { get; set; }
        public string TReference { get; set; }
        public string Notes { get; set; }
        public byte[] Attachment { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
    }
}
