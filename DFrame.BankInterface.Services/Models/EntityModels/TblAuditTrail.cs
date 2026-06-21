using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("TBL_AUDIT_TRAIL")]
    public class TblAuditTrail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("PROCESS_ID", TypeName = "int")]
        public int? ProcessId { get; set; }

        [Column("PROCESS_DESC", TypeName = "nvarchar(500)")]
        public string? ProcessDescription { get; set; }

        [Column("UNQ_ID", TypeName = "int")]
        public int? UniqueId { get; set; }

        [Column("ACTION_DESC", TypeName = "nvarchar(500)")]
        public string? ActionDescription { get; set; }

        [Column("ACTION_DATE", TypeName = "datetime")]
        public DateTime ActionDate { get; set; }

        [Column("COMMENTS", TypeName = "nvarchar(max)")]
        public string? Comments { get; set; }

        [Column("USER_ID", TypeName = "int")]
        public int? UserId { get; set; }

        [Column("USER_NAME", TypeName = "nvarchar(500)")]
        public string? UserName { get; set; }
    }
}
