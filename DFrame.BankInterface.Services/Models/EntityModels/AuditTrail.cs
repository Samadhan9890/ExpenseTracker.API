using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("AuditTrail")]
    public class AuditTrail
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("ClientId", TypeName = "int")]
        public int? ClientId { get; set; }

        [Column("SubscriptionId", TypeName = "int")]
        public int? SubscriptionId { get; set; }

        [Column("PaymentScheduleId", TypeName = "int")]
        public int? PaymentScheduleId { get; set; }

        [Column("ModuleName", TypeName = "varchar(30)")]
        [StringLength(50)]
        public string? ModuleName { get; set; }

        [Column("Message", TypeName = "varchar(500)")]
        [StringLength(500)]
        public string? Message { get; set; }

        [Column("Comments", TypeName = "varchar(500)")]
        [StringLength(500)]
        public string? Comments { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("CreatedBy", TypeName = "varchar(30)")]
        [StringLength(30)]
        public string? CreatedBy { get; set; }
    }
}

