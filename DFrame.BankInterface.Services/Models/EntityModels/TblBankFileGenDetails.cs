using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
	public class TblBankFileGenDetails
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Key]
		[MaxLength(30)]
		[Required]
		public string BankFileBatchId { get; set; }

		[MaxLength(200)]
		[Required]
		public string BankFilePath { get; set; }

		public DateTime BankFileGenDate { get; set; }

		[MaxLength(30)]
		[Required]
		public string BankFileGenBy { get; set; }

		[MaxLength(10)]
		public string? BankFileExt { get; set; }

		public int ProfileId { get; set; }
	}
}
