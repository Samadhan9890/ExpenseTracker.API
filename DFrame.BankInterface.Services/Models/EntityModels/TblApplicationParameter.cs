using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("TBL_APPLICATION_PARAMETER", Schema = "dbo")]
    public class TblApplicationParameter
    {

        [Column("PARAMETER_ID", TypeName = "int")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParameterId { get; set; }

        [Column("PARAMETER", TypeName = "varchar(max)")]
        public string? Parameter { get; set; }

        [Column("VALUE", TypeName = "varchar(max)")]
        public string? Value { get; set; }

        [Column("DESCRIPTION", TypeName = "varchar(max)")]
        public string? Description { get; set; }

        [Column("STATUS", TypeName = "int")]
        public int? Status { get; set; }

        [Column("ENTRY_ID", TypeName = "int")]
        public int? EntryId { get; set; }

        [Column("ENTRY_DATE")]
        public DateTime? EntryDate { get; set; }

        [Column("MODIFY_ID")]
        public int? ModifyId { get; set; }

        [Column("MODIFY_DATE")]
        public DateTime? ModifyDate { get; set; }

        [Column("DELETE_ID")]
        public int? DeleteId { get; set; }

        [Column("DELETE_DATE")]
        public DateTime? DeleteDate { get; set; }
    }
}
