using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Utilities.ConstantHelper;
using ExpenseTracker.Services.Utilities;
using System.Net.Mime;

namespace ExpenseTracker.Services.Repository.Generic
{
    public class GenericFunctions
    {
        private readonly AppDBContext _context;


        public GenericFunctions(AppDBContext context)
        {
            _context = context;
        }

        public async Task<TblAuditTrail> AddAuditTrail(int? buId, int? processId, string processDescription, int activityId, string activityDescription, int uniqueId, string actionDescription, string? comments, int? userId, string? userName, int? delegateId, string? delegateName)
        {
            var newAuditTrailRecord = new TblAuditTrail
            {
                ProcessId = processId,
                ProcessDescription = processDescription,
                UniqueId = uniqueId,
                ActionDescription = actionDescription,
                ActionDate = DateTime.Now,
                Comments = comments,
                UserId = userId,
                UserName = userName
            };

            await _context.AuditTrail.AddAsync(newAuditTrailRecord);
            await _context.SaveChangesAsync();

            return newAuditTrailRecord;
        }

        public async Task AddNcAuditTrail(int clientId, int? subscriptionId, int? paymentSchedId, string? moduleName, string? message, string? comments, string? createdby)
        {
            try
            {
                var auditTrail = new AuditTrail
                {
                    ClientId = clientId,
                    SubscriptionId = subscriptionId,
                    PaymentScheduleId = paymentSchedId,
                    ModuleName = moduleName,
                    Message = message,
                    Comments = comments,
                    CreatedBy = createdby


                };
                await _context.NcAuditTrail.AddAsync(auditTrail);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }


        }
    }
}
