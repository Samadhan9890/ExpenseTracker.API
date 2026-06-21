using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ExpenseTracker.Services.Models.EntityModels
{
	public class TblBankMaster
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Key]
		[MaxLength(20)]
		[Required]
		public string BankCode { get; set; }

		[MaxLength(80)]
		[Required]
		public string BankName { get; set; }

		[MaxLength(60)]
		[Required]
		public string BankAccNo { get; set; }

		[MaxLength(80)]
		public string? BankAdd01 { get; set; }

		[MaxLength(80)]
		public string? BankAdd02 { get; set; }

		[MaxLength(80)]
		public string? BankAdd03 { get; set; }

		[MaxLength(80)]
		public string? BankAdd04 { get; set; }

		[MaxLength(80)]
		public string? BankAdd05 { get; set; }

		[MaxLength(30)]
		public string? BankAccountType { get; set; }

		[MaxLength(20)]
		public string? BankIfscCode { get; set; }

		[MaxLength(50)]
		public string? BankEmailId { get; set; }

		[MaxLength(15)]
		public string? BankBranchCode { get; set; }

		[MaxLength(20)]
		public string? BankGlCode { get; set; }

		[MaxLength(20)]
		public string? BankClientCode { get; set; }

		[MaxLength(30)]
		public string SeriesDetails { get; set; }

        public DateTime EntryDate { get; set; }
        public string? EntryBy { get; set; }
    }
}
