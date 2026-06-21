using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.DTOs
{
	public class BankMasterDto
	{		
		public int Id { get; set; }
		[Required]
		public string BankCode { get; set; }
		[Required]
		public string BankName { get; set; }
		[Required]
		public string BankAccNo { get; set; }
		[Required]
		public string? BankAdd01 { get; set; }
		[Required]
		public string? BankAdd02 { get; set; }	
		public string? BankAdd03 { get; set; }		
		public string? BankAdd04 { get; set; }		
		public string? BankAdd05 { get; set; }		
		public string? BankAccountType { get; set; }		
		public string? BankIfscCode { get; set; }		
		public string? BankEmailId { get; set; }		
		public string? BankBranchCode { get; set; }		
		public string? BankGlCode { get; set; }		
		public string? BankClientCode { get; set; }		
		public string SeriesDetails { get; set; }
		public DateTime EntryDate { get; set; }
		public string? EntryBy { get; set; }
	}
}
