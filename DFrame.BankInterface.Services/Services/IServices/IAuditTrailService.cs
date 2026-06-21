using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Services.IServices
{
    public interface IAuditTrailService
    {
        Task<ResponseDto> GetAuditTrailAsync(string processIds, int unqId);

        Task<ResponseDto> GetNcAuditTrailAsync(int unqId, string module);
    }
}
