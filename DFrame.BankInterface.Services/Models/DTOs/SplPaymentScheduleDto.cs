using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseTracker.Services.Models.DTOs
{
    public class SplPaymentScheduleDto
    {
        public Guid Guid { get; set; }
        public int ScheduleId { get; set; }
        public int ClientId { get; set; }
        public int InvestmentId { get; set; }
        public int PaymentType { get; set; }
        public DateOnly DueDate { get; set; }
        public decimal? ProfitAmount { get; set; }
        public decimal? Tds { get; set; }
        public decimal? TdsPercent { get; set; }

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
        public string? ClientName { get;  set; }
        public string? PaidBy { get; set; }

        // Updated to handle multiple banking details
        
        public List<ClientBankingDetailsDto>? BankingDetails { get; set; }
    }
}
