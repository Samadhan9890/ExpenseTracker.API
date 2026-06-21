using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
	public class TblPaymentDetails
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public Guid GUID { get; set; }

		[MaxLength(15)]
		[Required]
		public string FmsSource { get; set; }

		[MaxLength(60)]
		public string? Treference { get; set; }

		public DateTime TransactionDate { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Amount { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Tax1 { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Tax2 { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Tax3 { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Tds { get; set; }

		[MaxLength(100)]
		public string? Tdescription { get; set; }

		[MaxLength(5)]
		public string? ContryCode { get; set; }

		[MaxLength(15)]
		public string? PaymentMode { get; set; }

		[MaxLength(5)]
		public string? CurrencyType { get; set; }

		[MaxLength(100)]
		public string? LocationDetails { get; set; }

		[MaxLength(20)]
		public string? TransactionType { get; set; }

		[MaxLength(20)]
		public string? JrnalNo { get; set; }

		public int JrnalLine { get; set; }

		[MaxLength(15)]
		public string? AllocRef { get; set; }

		[MaxLength(50)]
		public string? BeneSurName { get; set; }

		[MaxLength(50)]
		public string? BeneName { get; set; }

		[MaxLength(80)]
		public string? BeneAccNo { get; set; }

		[MaxLength(20)]
		public string? BeneIfscCode { get; set; }

		[MaxLength(100)]
		public string? BeneBankName { get; set; }

		[MaxLength(80)]
		public string? BeneAdd01 { get; set; }

		[MaxLength(80)]
		public string? BeneAdd02 { get; set; }

		[MaxLength(80)]
		public string? BeneAdd03 { get; set; }

		[MaxLength(80)]
		public string? BeneAdd04 { get; set; }

		[MaxLength(80)]
		public string? BeneAdd05 { get; set; }

		[MaxLength(50)]
		public string? BeneState { get; set; }

		[MaxLength(15)]
		public string? BenePhoneNo { get; set; }

		[MaxLength(50)]
		public string? BeneEmailId { get; set; }

		[MaxLength(15)]
		public string? BeneBranchCode { get; set; }

		[MaxLength(15)]
		public string? BeneMobileNo { get; set; }

		[MaxLength(30)]
		public string? BatchNo { get; set; }

		[MaxLength(1)]
		public string? ReversePostingStatus { get; set; }

		[MaxLength(30)]
		public string? ValidationRefNumber { get; set; }

		public DateTime ValidationDate { get; set; }

		public int ProcessId { get; set; }

		public int Status { get; set; }

		public int PreviousStatus { get; set; }

		public DateTime EntryDate { get; set; }

		[MaxLength(15)]
		public string? EntryBy { get; set; }

		public DateTime ModifiedDate { get; set; }

		[MaxLength(15)]
		public string? ModifiedBy { get; set; }

		[MaxLength(20)]
		public string? BankCode { get; set; }

		[MaxLength(30)]
		public string? BankFileBatchId { get; set; }

		[MaxLength(60)]
		public string? UnqRefNo { get; set; }

		[MaxLength(80)]
		public string? Udf1 { get; set; }

		[MaxLength(80)]
		public string? Udf2 { get; set; }

		[MaxLength(80)]
		public string? Udf3 { get; set; }

		[MaxLength(80)]
		public string? Udf4 { get; set; }

		[MaxLength(80)]
		public string? Udf5 { get; set; }

		[MaxLength(30)]
		public string? PmtUtrDetails { get; set; }

		[MaxLength(30)]
		public string? PmtStatus { get; set; }

		[MaxLength(80)]
		public string? PmtRemarks { get; set; }

		public DateTime PmtDate { get; set; }

		[MaxLength(100)]
		public string? PmtRejectionReason { get; set; }

		[MaxLength(50)]
		public string? PmtUnqRefNo { get; set; }

		[MaxLength(60)]
		public string? PmtRespFileId { get; set; }

		[NotMapped]
		[ForeignKey("BankCode")]
		public TblBankMaster? BankMaster { get; set; }

		[NotMapped]
		[ForeignKey("BankFileBatchId")]
		public TblBankFileGenDetails? BankFileGenDetails { get; set; }
	}
}
