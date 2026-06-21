namespace ExpenseTracker.Services.Models.DTOs
{
    public class CustomReportMasterDto
    {
        public int CustomReportId { get; set; }
        public string CustomReportCode { get; set; }
        public string CustomReportName { get; set; }
        public string CustomReportDesc { get; set; }
        public string CustomReportQuery { get; set; }
        public int? Status { get; set; }
        public int? EntryId { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? DeleteId { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string RoleAccess { get; set; }
        public int? IsDag { get; set; }
        public string? DateFilter { get; set; }
        public string? OrderFilter { get; set; }
        public string? DagDesc { get; set; }
        public string? ColumnFilter { get; set; }
    }
}
