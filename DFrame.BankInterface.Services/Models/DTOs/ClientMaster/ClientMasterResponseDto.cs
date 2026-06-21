using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Services.Models.DTOs.ClientMaster
{
	public class ClientMasterResponseDto
	{
		public int ClientId { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string AadharNo { get; set; }

		public string? PanNo { get; set; }
		public DateTime? DOB { get; set; }
		public string? ReferredBy { get; set; }
		[Required]
		public string PerAddressLine1 { get; set; }
		[Required]
		public string PerAddressLine2 { get; set; }
		public string? PerAddressLine3 { get; set; }
		[Required]
		public string PerState { get; set; }
		[Required]
		public string PerCity { get; set; }
		[Required]
		public int PerPinCode { get; set; }
		[Required]
		public string MailAddressLine1 { get; set; }
		[Required]
		public string MailAddressLine2 { get; set; }
		public string? MailAddressLine3 { get; set; }
		[Required]
		public string MailState { get; set; }
		[Required]
		public string MailCity { get; set; }
		[Required]
		public int MailPinCode { get; set; }
		[Phone]
		public string? Phone { get; set; }
		[Required]
		[Phone]
		public string Mobile { get; set; }
		[EmailAddress]
		public string? Email { get; set; }
		public string? AadharAttachmentPath { get; set; }
		public string? PanAttachmentPath { get; set; }
		public string? ProfileImageAttachmentPath { get; set; }
		public string? FamilyTag { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string? CreatedBy { get; set; }
		public bool? Status { get; set; }
		public string? Notes { get; set; }

        public byte[] ProfilePicDoc { get; set; }
        public byte[] AadharCardDoc { get; set; }
        public byte[] PanCardDoc { get; set; }
        public string? OfficeAddressLine1 { get; set; }
        public string? OfficeAddressLine2 { get; set; }
        public string? OfficeAddressLine3 { get; set; }
        public string? OfficeState { get; set; }
        public string? OfficeCity { get; set; }
        public int? OfficePinCode { get; set; }
        public string? BloodRelation { get; set; }
        public bool IsReferralBonusApplicable { get; set; }
        public bool IsAadharPanLinked { get; set; }
        public List<ClientBankingDetailsDto>? LstClientBankingDetails { get; set; }

    }
}
