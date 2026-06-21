using System.Data;

namespace ExpenseTracker.Services.Models.ServiceModels
{
    public class FilterSearchCriteria
    {
        public int? Id { get; set; }
        public string? Label { get; set; }
        public string? Value { get; set; }
    }

    public class ReportViewerModel
    {
        public List<FilterSearchCriteria> ListSearchCriteria1 { get; set; } = new List<FilterSearchCriteria>();
        public List<FilterSearchCriteria> ListSearchCriteria2 { get; set; } = new List<FilterSearchCriteria>();
        public List<FilterSearchCriteria> ListSearchCriteria3 { get; set; } = new List<FilterSearchCriteria>();
    }

    public class ReportViewerFilter
    {
        public string? Code1 { get; set; }
        public string? Code2 { get; set; }
        public string? Date1 { get; set; }
        public string? Date2 { get; set; }
        public string? SearchText1 { get; set; }
        public string? SearchText2 { get; set; }
        public string? SearchDate { get; set; }
    }

    public class ReportExcelModel
    {
        public DataTable? Data { get; set; }
        public string ReportName { get; set; }
        public string? ReportPath { get; set; }
        public string? SystemTempPath { get; set; }
    }
}
