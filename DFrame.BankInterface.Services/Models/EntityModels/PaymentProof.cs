using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Net.Mail;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("TBL_PAYMENT_PROOF")]
    public class PaymentProof
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int Id { get; set; }

        [Required]
        [Column("PAYMENT_SCHEDULE_GUID", TypeName = "uniqueidentifier")]
        public Guid PaymentScheduleGuid { get; set; }

        [Required]
        [Column("TREFERENCE", TypeName = "varchar(50)")]
        public string TReference { get; set; }

        [Column("NOTES", TypeName = "varchar(500)")]
        public string Notes { get; set; }

        [Required]
        [Column("ATTACHMENT", TypeName = "varbinary(max)")]
        public byte[] Attachment { get; set; }

        [Column("ORIGINAL_FILE_NAME", TypeName = "varchar(200)")]
        public string OriginalFileName { get; set; }

        [Required]
        [Column("CREATED_DATE", TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Column("CREATED_BY", TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }
    }
}
