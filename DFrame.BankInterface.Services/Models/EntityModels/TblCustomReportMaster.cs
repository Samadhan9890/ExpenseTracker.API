using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.EntityModels
{
    [Table("TBL_CUSTOM_REPORT_MASTER", Schema = "dbo")]
    public class TblCustomReportMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CUSTOM_REPORT_ID", TypeName = "int")]
        public int CustomReportId { get; set; }
        [Column("CUSTOM_REPORT_CODE", TypeName = "varchar(200)")]
        public string CustomReportCode { get; set; }
        [Column("CUSTOM_REPORT_NAME", TypeName = "varchar(200)")]
        public string CustomReportName { get; set; }
        [Column("CUSTOM_REPORT_DESC", TypeName = "varchar(max)")]
        public string CustomReportDesc { get; set; }
        [Column("CUSTOM_REPORT_QUERY", TypeName = "varchar(max)")]
        public string CustomReportQuery { get; set; }
        [Column("STATUS", TypeName = "int")]
        public int? Status { get; set; }
        [Column("ENTRY_ID", TypeName = "int")]
        public int? EntryId { get; set; }
        [Column("ENTRY_DATE", TypeName = "datetime")]
        public DateTime? EntryDate { get; set; }
        [Column("MODIFY_ID", TypeName = "int")]
        public int? ModifyId { get; set; }
        [Column("MODIFY_DATE", TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
        [Column("DELETE_ID", TypeName = "int")]
        public int? DeleteId { get; set; }
        [Column("DELETE_DATE", TypeName = "datetime")]
        public DateTime? DeleteDate { get; set; }
        [Column("ROLE_ACCESS", TypeName = "varchar(max)")]
        public string RoleAccess { get; set; }
        [Column("ISDAG", TypeName = "int")]
        public int? IsDag { get; set; }
        [Column("DATE_FILTER", TypeName = "varchar(200)")]
        public string? DateFilter { get; set; }
        [Column("ORDER_FILTER", TypeName = "varchar(max)")]
        public string? OrderFilter { get; set; }
        [Column("DAG_DESC", TypeName = "varchar(50)")]
        public string? DagDesc { get; set; }
        [Column("COLUMN_FILTER", TypeName = "varchar(max)")]
        public string? ColumnFilter { get; set; }
    }
}
