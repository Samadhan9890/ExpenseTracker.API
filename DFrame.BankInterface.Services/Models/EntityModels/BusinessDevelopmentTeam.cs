using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Services.Models.DTOs;

namespace ExpenseTracker.Services.Models.EntityModels
{

    [Table("TBL_BUSINESS_DEV_TEAM")]
    public class BusinessDevTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("BDID", TypeName = "int")]
        public int BDId { get; set; }

        [Column("ClientId", TypeName = "int")]
        public int? ClientId { get; set; }

        [Required]
        [Column("NAME", TypeName = "varchar(255)")]
        public string Name { get; set; }

        [Column("PanNo", TypeName = "varchar(12)")]
        public string PanNo { get; set; }

        [Column("AadharNo", TypeName = "varchar(15)")]
        public string AadharNo { get; set; }

        [Column("ADDRESS", TypeName = "varchar(255)")]
        public string Address { get; set; }

        [Column("JOINING_DATE", TypeName = "date")]
        public DateTime JoiningDate { get; set; }

        [Column("ENTRY_DATE", TypeName = "date")]
        public DateTime EntryDate { get; set; }

        [Column("IsClient", TypeName = "bit")]
        public bool IsClient { get; set; }

        [Column("STATUS", TypeName = "bit")]
        public bool Status { get; set; }

        [NotMapped]
        public List<ClientBankingDetail> BankingDetails { get; set; } = new List<ClientBankingDetail>();

    }

}
