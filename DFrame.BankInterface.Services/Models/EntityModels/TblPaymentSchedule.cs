using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("PAYMENT_SCHEDULE")]
    public class TblPaymentSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("SCHEDULE_ID")]
        public int ScheduleId { get; set; }

		[Column("GUID", TypeName = "uniqueidentifier")]
		public Guid Guid { get; set; }

        [ForeignKey("ClientMaster")]
        public int ClientId { get; set; }
        public ClientMaster ClientMaster { get; set; }

        [ForeignKey("TblSubscription")]
        public int SubscriptionId { get; set; }
        public TblSubscription Subscription { get; set; }

        [Column("DUE_DATE", TypeName = "datetime")]
        public DateTime? DueDate { get; set; }

        [Column("PAYABLE_AMOUNT", TypeName = "decimal(18,2)")]
        public decimal? PayableAmount { get; set; }

        [Column("AMOUNT_PAID", TypeName = "decimal(18,2)")]
        public decimal? AmountPaid { get; set; }

        [Column("PAYMENT_DATE", TypeName = "datetime")]
        public DateTime? PaymentDate { get; set; }

        [Column("PAYMENT_MODE", TypeName = "varchar(30)")]
        public string? PaymentMode { get; set; }

		[Column("PAID_BY", TypeName = "varchar(50)")]
		public string? PaidBy { get; set; }

		[Column("PAYMENT_UTR", TypeName = "varchar(30)")]
        public string? PaymentUtr { get; set; }

        [Column("PAYMENT_PROOF_ATTACHMENT", TypeName = "varchar(255)")]
        public string? PaymentProofAttachment { get; set; }

        [Column("STATUS")]
        public int? Status { get; set; } = 0;

        [Column("NOTES", TypeName = "varchar(255)")]
        public string? Notes { get; set; }

        [Column("CREATED_DATE", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [Column("CREATED_BY", TypeName = "varchar(50)")]
        public string? CreatedBy { get; set; }

        [Column("INTEREST_RATE", TypeName = "decimal(5,2)")]
        public decimal? InterestRate { get; set; }

        [Column("INVESTED_AMOUNT", TypeName = "decimal(18,2)")]
        public decimal? InvestedAmount { get; set; }

        [Column("DAY", TypeName = "varchar(20)")]
        public string? Day { get; set; }
    }
}
