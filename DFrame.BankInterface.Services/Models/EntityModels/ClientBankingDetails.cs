namespace ExpenseTracker.Services.Models.EntityModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ClientBankingDetails")]
    public class ClientBankingDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("ClientId")]
        public int ClientId { get; set; }

        [Column("InvestmentId")]
        public int InvestmentId { get; set; }

        [Column("InvestmentGuid")]
        public Guid InvestmentGuid { get; set; }

        [Column("Mode", TypeName = "varchar(10)")]
        public string Mode { get; set; }

        [Column("AccountNoOrUpiId", TypeName = "varchar(50)")]
        public string AccountNoOrUpiId { get; set; }

        [Column("BankName", TypeName = "varchar(40)")]
        public string? BankName { get; set; }

        [Column("AccountHolderName", TypeName = "varchar(50)")]
        public string? AccountHolderName { get; set; }

        [Column("IFSCCode", TypeName = "varchar(20)")]
        public string? IFSCCode { get; set; }

        [Column("Status")]
        public bool Status { get; set; }

        [Column("CreatedDate", TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [Column("CreatedBy", TypeName = "varchar(30)")]
        public string CreatedBy { get; set; }

        // New property to store the type of banking details ("Referral" or "Client")
        [Column("BankingType", TypeName = "varchar(100)")]
        public string BankingType { get; set; }

        [Column("BusinessDevTeamId")]
        public int BusinessDevTeamId { get; set; }

        [Column("Note")]
        public string? Note { get; internal set; }
    }

}
