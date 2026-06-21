using ExpenseTracker.Services.Models.EntityModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Services.Models.DTOs
{
    public class PaymentScheduleDto
    {
        public Guid Guid { get; set; } 
        public int ScheduleId { get; set; }
        public int ClientId { get; set; }
        public int SubscriptionId { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? PayableAmount { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PaymentMode { get; set; }
        public string? PaymentUtr { get; set; }
        public string? PaymentProofAttachment { get; set; }
        public int? Status { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? InvestedAmount { get; set; }
        public string? Day { get; set; }
        public string? ClientName { get; internal set; }
		public string? PrefferedPaymentMode { get; set; }
		public string? PrefferedPayoutBankName { get; set; }
        public string? PrefferedPayoutBankAccountNo { get; set; }
        public string? PrefferedPayoutBankIfscCode { get; set; }
        public string? PrefferedPayoutBankAccountHolderName { get; set; }
        public string? PrefferedUpiId { get; set; }
        public string? PaidBy { get; set; }
    }
}
