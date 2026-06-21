using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("TBL_DEPARTMENT", Schema = "dbo")]
    public class TblDepartment
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Column("DEPARTMENT_ID", TypeName = "int")]
            public int DepartmentId { get; set; }

            [Column("DEPARTMENT_CODE")]
            public string? DepartmentCode { get; set; }

            [Column("DEPARTMENT_DESCRIPTION")]
            public string? DepartmentDescription { get; set; }

            [Column("SUN_ACCOUNT_CODE")]
            public string? SunAccountCode { get; set; }

            [Column("STATUS", TypeName = "bit")]
            public bool? Status { get; set; }

            [Column("ENTRY_ID")]
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

            [Column("L_DEP_ONLY")]
            public bool? IDepOnly { get; set; }
        }
}
