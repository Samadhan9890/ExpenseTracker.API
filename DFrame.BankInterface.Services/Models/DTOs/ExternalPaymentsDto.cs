namespace ExpenseTracker.Services.Models.DTOs
{
    public class ExternalPaymentsDto
    {
        public int Id { get; set; }
        public string TransferredFrom { get; set; }
        public string TransferredTo { get; set; }
        public decimal Amount { get; set; }
        public string DrCr { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
