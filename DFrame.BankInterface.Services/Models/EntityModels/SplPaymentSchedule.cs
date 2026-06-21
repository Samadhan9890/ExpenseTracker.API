using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("SPL_PAYMENT_SCHEDULE")]
    public class SplPaymentSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("SCHEDULE_ID")]
        public int ScheduleId { get; set; }

        [Column("GUID")]
        public Guid Guid { get; set; }

        [Column("CLIENT_ID")]
        public int ClientId { get; set; }

        [Column("INVESTMENT_ID")]
        public int InvestmentId { get; set; }

        [Column("PAYMENT_TYPE")]
        public int PaymentType { get; set; }

        [Column("DUE_DATE", TypeName = "date")]
        public DateOnly DueDate { get; set; }

        [Column("PROFIT_AMOUNT", TypeName = "decimal(18,2)")]
        public decimal? ProfitAmount { get; set; }

        [Column("TDS", TypeName = "decimal(18,2)")]
        public decimal? Tds { get; set; }

        [Column("PAYABLE_AMOUNT", TypeName = "decimal(18,2)")]
        public decimal? PayableAmount { get; set; }

        [Column("AMOUNT_PAID", TypeName = "decimal(18,2)")]
        public decimal? AmountPaid { get; set; }

        [Column("PAYMENT_DATE", TypeName = "datetime")]
        public DateTime? PaymentDate { get; set; }

        [Column("PAYMENT_MODE", TypeName = "varchar(20)")]
        public string? PaymentMode { get; set; }

        [Column("PAYMENT_UTR", TypeName = "varchar(100)")]
        public string? PaymentUtr { get; set; }

        [Column("PAYMENT_PROOF_ATTACHMENT", TypeName = "varchar(255)")]
        public string? PaymentProofAttachment { get; set; }

        [Column("STATUS")]
        public int? Status { get; set; }

        [Column("NOTES", TypeName = "varchar(500)")]
        public string? Notes { get; set; }

        [Column("CREATED_DATE", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [Column("CREATED_BY", TypeName = "varchar(50)")]
        public string? CreatedBy { get; set; }

        [Column("INTEREST_RATE", TypeName = "decimal(18,2)")]
        public decimal? InterestRate { get; set; }

        [Column("INVESTED_AMOUNT", TypeName = "decimal(18,2)")]
        public decimal? InvestedAmount { get; set; }

        [Column("DAY", TypeName = "varchar(10)")]
        public string? Day { get; set; }

        [Column("CLIENT_NAME", TypeName = "varchar(200)")]
        public string? ClientName { get; set; }

        [Column("PAID_BY", TypeName = "varchar(50)")]
        public string? PaidBy { get; set; }

        // Navigation properties
        public ClientMaster ClientMaster { get; set; }
        public Investment Investment { get; set; }
    }

}
