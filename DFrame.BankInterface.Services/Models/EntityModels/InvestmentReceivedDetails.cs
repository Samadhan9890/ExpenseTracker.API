namespace ExpenseTracker.Services.Models.EntityModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("InvestmentReceivedDetails")]
    public class InvestmentReceivedDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("ProcessId")]
        public int ProcessId { get; set; }

        [Column("InvestmentId")]
        public int InvestmentId { get; set; }

        [Column("ReceivedDate", TypeName = "date")]
        public DateOnly ReceivedDate { get; set; }

        [Column("InvestmentGuid")]
        public Guid InvestmentGuid { get; set; }

        [Column("Mode", TypeName = "varchar(10)")]
        public string Mode { get; set; }

        [Column("BankName", TypeName = "varchar(50)")]
        public string? BankName { get; set; }

        [Column("AccountNumberOrUpiId", TypeName = "varchar(30)")]
        public string? AccountNumberOrUpiId { get; set; }

        [Column("Amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column("Comments", TypeName = "varchar(255)")]
        public string? Comments { get; set; }

        [Column("AddedBy", TypeName = "varchar(30)")]
        public string AddedBy { get; set; }

        [Column("CreatedDate", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }


        [Column("Status")]
        public bool Status { get; set; }

        [Column("InvestmentAttachment", TypeName = "varbinary(max)")]
        public byte[]? InvestmentAttachment { get; set; }
    }

}
