using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Utilities.ConstantHelper;
using static ExpenseTracker.Services.Utilities.ConstantHelper.ConstantHelper;

namespace ExpenseTracker.Services.Repository
{
    public class AuditTrailRepository : IAuditTrailRepository
    {
        private readonly AppDBContext _dbContext;
        public AuditTrailRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<List<TblAuditTrail>> GetAuditTrailAsync(string processIds, int unqId)
        {
            // Split the processIds string into an array of integers
            List<int> processIdList = processIds.Split(',').Select(int.Parse).ToList();

            List<TblAuditTrail> response = await _dbContext.AuditTrail.AsNoTracking()
                .Where(a => processIdList.Contains((int)a.ProcessId) && a.UniqueId == unqId).OrderBy(o => o.ActionDate).ToListAsync();
            return response;
        }

        public async Task<List<AuditTrail>> GetNcAuditTrailAsync(int unqId, string module)
        {
            List<AuditTrail> ncAudits = new List<AuditTrail>();

            if (module == NCAuditModulesEnum.CLIENT_MASTER.ToString())
            {
                ncAudits = await _dbContext.NcAuditTrail.AsNoTracking().Where(a => a.ClientId == unqId).OrderByDescending(c=> c.CreatedDate).ToListAsync();
            }
            else if(module == NCAuditModulesEnum.SUBSCRIPTION.ToString())
            {
                ncAudits = await _dbContext.NcAuditTrail.AsNoTracking().Where(a => a.SubscriptionId == unqId).OrderByDescending(c => c.CreatedDate).ToListAsync();
            }
            else if (module == NCAuditModulesEnum.SPL_INVESTMENTS.ToString())
            {
                ncAudits = await _dbContext.NcAuditTrail.AsNoTracking().Where(a => a.SubscriptionId == unqId).OrderByDescending(c => c.CreatedDate).ToListAsync();
            }
            else if (module == NCAuditModulesEnum.PAYMENTS.ToString())
            {
                ncAudits = await _dbContext.NcAuditTrail.AsNoTracking().Where(a => a.PaymentScheduleId == unqId).OrderByDescending(c => c.CreatedDate).ToListAsync();
            }
            else if (module == NCAuditModulesEnum.SPL_PAYMENTS.ToString())
            {
                ncAudits = await _dbContext.NcAuditTrail.AsNoTracking().Where(a => a.PaymentScheduleId == unqId).OrderByDescending(c => c.CreatedDate).ToListAsync();
            }

            return ncAudits;

        }
    }
}
