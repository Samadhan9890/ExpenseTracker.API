using ExpenseTracker.Services.Contracts;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.ServiceModels;

namespace ExpenseTracker.Services.Services.IServices
{
    public interface IReportMasterService
    {
        Task<ResponseDto> GetReportManagerList();
        Task<ResponseDto> GetReportManagerDataById(int id);
        Task<ResponseDto> AddUpdateReport(CustomReportMasterDto reportData, UserClaims userDetails);
        Task<ResponseDto> DeleteReportMaster(int id);
        Task<ResponseDto> GetReportViewerList(UserClaims userDetails);
        Task<ResponseDto> GetReportViewerDataById(int id, UserClaims userDetails);
        Task<ResponseDto> GenerateExcelForReport(int id, ReportViewerFilter reportView, UserClaims userDetails);
    }
}
