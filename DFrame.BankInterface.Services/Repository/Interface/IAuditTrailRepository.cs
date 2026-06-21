using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Utilities.ConstantHelper;
using static ExpenseTracker.Services.Utilities.ConstantHelper.ConstantHelper;

namespace ExpenseTracker.Services.Repository.Interface
{
    public interface IAuditTrailRepository
    {
        Task<List<TblAuditTrail>> GetAuditTrailAsync(string processIds, int unqId);

        Task<List<AuditTrail>> GetNcAuditTrailAsync(int unqId, string module);
    }
}
