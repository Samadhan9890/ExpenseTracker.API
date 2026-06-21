using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Services.Models.EntityModels
{
	[Table("TBL_BORROW_LETTER_DETAILS")]
	public class TblBorrowLetterDetails
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("ID")]
		public int Id { get; set; }

		[Column("SUBSCRIPTION_GUID", TypeName = "uniqueidentifier")]
		public Guid SubscriptionGuid { get; set; }

		[Column("SUBSCRIPTION_ID", TypeName = "int")]
		public int SubscriptionId { get; set; }

		[Column("CHEQUE_NO", TypeName = "varchar(50)")]
		public string? ChequeNo { get; set; }

		[Column("CHEQUE_DATE", TypeName = "varchar(50)")]
		public string? ChequeDate { get; set; }

		[Column("SENT_DATE", TypeName = "varchar(50)")]
		public string? SentDate { get; set; }
		[Column("TRACKING_NO", TypeName = "varchar(50)")]
		public string? TrackingNo { get; set; }

    }
}
