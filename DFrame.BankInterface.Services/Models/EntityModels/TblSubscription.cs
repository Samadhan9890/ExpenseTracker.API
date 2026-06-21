using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
	[Table("SUBSCRIPTION")]
	public class TblSubscription
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("SUBSCRIPTION_ID")]
		public int SubscriptionId { get; set; }

		[Column("GUID", TypeName = "uniqueidentifier")]
		public Guid Guid { get; set; }

		[Column("SUBSCRIPTION_TYPE", TypeName = "varchar(255)")]
		public string SubscriptionType { get; set; }

		[Column("OLD_SUBSCRIPTION_ID")]
		public int? OldSubscriptionId { get; set; }

		[Column("CLIENT_ID")]
		public int ClientId { get; set; }

		[Column("DATE_OF_INVESTMENT", TypeName = "datetime")]
		public DateTime DateOfInvestment { get; set; }

		[Column("PLAN_CODE", TypeName = "varchar(30)")]
		public string PlanCode { get; set; }

		[Column("PLAN_NAME", TypeName = "nvarchar(max)")]
		public string PlanName { get; set; }

		[Column("INVESTMENT_AMOUNT", TypeName = "decimal(18,2)")]
		public decimal InvestmentAmount { get; set; }

		[Column("PAYOUT_FREQUENCY")]
		public int PayoutFrequency { get; set; }

		[Column("TOTAL_INTEREST", TypeName = "decimal(18,2)")]
		public decimal TotalInterest { get; set; }

		[Column("PAYOUT_FREQUENCY_INTEREST_RATE", TypeName = "decimal(5,2)")]
		public decimal PayoutFrequencyInterestRate { get; set; }

		[Column("MATURITY_DATE", TypeName = "datetime")]
		public DateTime MaturityDate { get; set; }

		[Column("TENURE")]
		public int Tenure { get; set; }

        [Column("APPROVED_BY", TypeName = "varchar(255)")]
		public string? ApprovedBy { get; set; }

		[Column("APPROVED_DATE", TypeName = "datetime")]
		public DateTime? ApprovedDate { get; set; }

		[Column("BORROW_LETTER_STATUS", TypeName = "int")]
		public int BorrowLetterStatus { get; set; }

		[Column("PAYOUT_METHOD", TypeName = "varchar(255)")]
		public string? PayoutMethod { get; set; }

		[Column("PAYOUT_BANK_NAME", TypeName = "varchar(255)")]
		public string? PayoutBankName { get; set; }

		[Column("PAYOUT_BANK_ACCOUNT_NO")]
		public string? PayoutBankAccountNo { get; set; }

		[Column("PAYOUT_BANK_IFSC_CODE", TypeName = "varchar(255)")]
		public string? PayoutBankIfscCode { get; set; }

		[Column("PAYOUT_BANK_ACCOUNT_HOLDER_NAME", TypeName = "varchar(255)")]
		public string? PayoutBankAccountHolderName { get; set; }

		[Column("UPI_ID", TypeName = "varchar(255)")]
		public string? UpiId { get; set; }

		[Column("NOTES", TypeName = "varchar(255)")]
		public string? Notes { get; set; }

		[Column("STATUS")]
		public int Status { get; set; }

		[Column("CREATED_DATE", TypeName = "datetime")]
		public DateTime CreatedDate { get; set; }

		[Column("CREATED_BY", TypeName = "varchar(255)")]
		public string CreatedBy { get; set; }

		[Column("IS_PAYMENT_SCEDULE_AVAILABLE", TypeName = "bit")]
		public bool IsPaymentScheduleAvailable { get; set; }

		[Column("ClientMasterClientId", TypeName = "int")]
		public int ClientMasterClientId { get; set; }

		[Column("CLOSING_DATE", TypeName = "datetime")]
		public DateTime? ClosingDate { get; set; }

		[Column("CLOSED_BY", TypeName = "varchar(50)")]
		public string? ClosedBy { get; set; }

		[Column("INVESTMENT_RECVD_DETAILS", TypeName = "varchar(300)")]
		public string? InvestmentReceivedDetails { get; set; }


		[Column("INVESTMENT_RCVD_DATE", TypeName = "datetime")]
		public DateTime? InvestmentReceivedDate { get; set; }

        [Column("LAST_UPDATED_BY", TypeName = "varchar(40)")]
        public string? LastUpdatedBy { get; set; }

        [NotMapped]
        public bool IsActionable { get; set; }

        // Navigation properties
        [NotMapped]
		public virtual ClientMaster ClientMaster { get; set; }
		[NotMapped]
		public virtual PlanMaster PlanMaster { get; set; }

        [Column("RECEIVED_INVESTMENT_METHOD", TypeName = "Varchar(50)")]
        public string? ReceivedInvestmentMethod { get; set; }
    }


}
