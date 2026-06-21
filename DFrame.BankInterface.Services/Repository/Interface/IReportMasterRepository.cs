using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Models.ServiceModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExpenseTracker.Services.Repository.Interface
{
    public interface IReportMasterRepository
    {
        Task<List<TblCustomReportMaster>> GetReportManagerList();
        Task<TblCustomReportMaster> GetReportManagerDataById(int id);
        Task<TblCustomReportMaster> AddUpdateReport(TblCustomReportMaster tblCustomReport, TblUser user);
        Task DeleteReportMaster(int id);
        Task<List<TblCustomReportMaster>> GetReportViewerList(TblUser user);
        Task<ReportViewerModel> GetReportViewerDataById(int id, TblUser user);
        Task<ReportExcelModel> GenerateExcelForReport(int id, ReportViewerFilter reportView, TblUser user);
    }
}