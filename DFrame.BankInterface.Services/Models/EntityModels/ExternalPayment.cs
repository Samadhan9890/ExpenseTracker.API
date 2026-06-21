using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("ExternalPayments")]
    public class ExternalPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("TransferredFrom", TypeName = "varchar(500)")]
        public string TransferredFrom { get; set; }

        [Column("TransferredTo", TypeName = "varchar(500)")]
        public string TransferredTo { get; set; }

        [Column("Amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column("DrCr", TypeName = "varchar(10)")]
        public string DrCr { get; set; } // DR/CR

        [Column("TransactionDate", TypeName = "datetime")]
        public DateTime TransactionDate { get; set; }

        [Column("Description", TypeName = "varchar(500)")]
        public string Description { get; set; }

        [Column("Status")]
        public int Status { get; set; }

        [Column("CreatedDate", TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [Column("CreatedBy", TypeName = "varchar(30)")]
        public string CreatedBy { get; set; }
    }
}
