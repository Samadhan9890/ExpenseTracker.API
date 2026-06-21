using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseTracker.Services.Models.EntityModels
{
	[Table("CLIENT_MASTER")]
	public class ClientMaster
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("CLIENT_ID")]
		public int ClientId { get; set; }
		[Column("GUID", TypeName = "uniqueidentifier")]
		public Guid Guid { get; set; }

		[Column("NAME", TypeName = "varchar(255)")]
		public string Name { get; set; }

		[Column("AADHAR_NO", TypeName = "varchar(20)")]
		public string AadharNo { get; set; }

		[Column("PAN_NO", TypeName = "varchar(10)")]
		public string? PanNo { get; set; }

		[Column("DOB", TypeName = "date")]
		public DateTime? DOB { get; set; }

		[Column("REFERRED_BY", TypeName = "varchar(30)")]		
		public string? ReferredBy { get; set; }

		[Column("PER_ADDRESS_LINE1", TypeName = "varchar(60)")]
		public string PerAddressLine1 { get; set; }

		[Column("PER_ADDRESS_LINE2", TypeName = "varchar(60)")]
		public string? PerAddressLine2 { get; set; }

		[Column("PER_ADDRESS_LINE3", TypeName = "varchar(60)")]
		public string? PerAddressLine3 { get; set; }

		[Column("PER_STATE", TypeName = "varchar(50)")]
		public string PerState { get; set; }

		[Column("PER_CITY", TypeName = "varchar(50)")]
		public string PerCity { get; set; }

		[Column("PER_PIN_CODE")]
		public int PerPinCode { get; set; }

		[Column("MAIL_ADDRESS_LINE1", TypeName = "varchar(60)")]
		public string MailAddressLine1 { get; set; }

		[Column("MAIL_ADDRESS_LINE2", TypeName = "varchar(60)")]
		public string? MailAddressLine2 { get; set; }

		[Column("MAIL_ADDRESS_LINE3", TypeName = "varchar(60)")]
		public string? MailAddressLine3 { get; set; }

		[Column("MAIL_STATE", TypeName = "varchar(50)")]
		public string MailState { get; set; }

		[Column("MAIL_CITY", TypeName = "varchar(50)")]
		public string MailCity { get; set; }

		[Column("MAIL_PIN_CODE")]
		public int MailPinCode { get; set; }

		[Column("PHONE", TypeName = "varchar(15)")]
		public string? Phone { get; set; }

		[Column("MOBILE", TypeName = "varchar(15)")]
		public string Mobile { get; set; }

		[Column("EMAIL", TypeName = "varchar(80)")]
		public string? Email { get; set; }

		[Column("AADHAR_ATTACHMENT_PATH", TypeName = "varchar(255)")]
		public string? AadharAttachmentPath { get; set; }

		[Column("PAN_ATTACHMENT_PATH", TypeName = "varchar(255)")]
		public string? PanAttachmentPath { get; set; }

		[Column("PROFILE_IMAGE_ATTACHMENT_PATH", TypeName = "varchar(255)")]
		public string? ProfileImageAttachmentPath { get; set; }

		[Column("FAMILY_TAG", TypeName = "varchar(100)")]
		public string? FamilyTag { get; set; }

		[Column("CREATED_DATE", TypeName = "datetime")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CreatedDate { get; set; }

		[Column("CREATED_BY", TypeName = "varchar(50)")]
		public string CreatedBy { get; set; }

		[Column("STATUS")]
		public bool Status { get; set; }

		[Column("NOTES", TypeName = "varchar(255)")]
		public string? Notes { get; set; }

        [NotMapped]
        public virtual ICollection<TblSubscription> Subscriptions { get; set; }

        [Column("OFFICE_ADDRESS_LINE1", TypeName = "varchar(255)")]
        public string? OfficeAddressLine1 { get; set; }
        [Column("OFFICE_ADDRESS_LINE2", TypeName = "varchar(255)")]
        public string? OfficeAddressLine2 { get; set; }
        [Column("OFFICE_ADDRESS_LINE3", TypeName = "varchar(255)")]
        public string? OfficeAddressLine3 { get; set; }
        [Column("OFFICE_STATE", TypeName = "varchar(255)")]
        public string? OfficeState { get; set; }
        [Column("OFFICE_CITY", TypeName = "varchar(255)")]
        public string? OfficeCity { get; set; }
        [Column("OFFICE_PIN_CODE", TypeName = "varchar(255)")]
        public int? OfficePinCode { get; set; }

        [Column("BLOOD_RELATION", TypeName = "varchar(255)")] // New column for Blood Relation
        public string? BloodRelation { get; set; }

        [Column("IS_REFERRAL_BONUS_APPLICABLE")]
        public bool IsReferralBonusApplicable { get; set; } // New column for Referral Bonus checkbox

        [Column("IS_AADHAR_PAN_LINKED")]
        public bool IsAadharPanLinked { get; set; } // New column for Aadhar-PAN linked checkbox

        [NotMapped]
        public virtual ICollection<Investment> Investments { get; set; }
        [NotMapped]
        public List<ClientBankingDetail> LstClientBankingDetails { get; set; }


        [NotMapped]
        public string? RefferedByName { get; set; }

    }
}
