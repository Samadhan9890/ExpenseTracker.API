using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;

namespace ExpenseTracker.Services.Repository
{
    public class PaymentProofRepository : IPaymentProofRepository
    {
        private readonly AppDBContext _context;
        private readonly ILogger<PaymentProofRepository> _logger;


        public PaymentProofRepository(AppDBContext context, ILogger<PaymentProofRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<PaymentProof> AddpaymentProofAsync(PaymentProof paymentProof)
        {
            try
            {
                _context.paymentProofs.Add(paymentProof);
                await _context.SaveChangesAsync();
                return paymentProof;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while adding a payment proof: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<PaymentProof>> GetPaymentProofByPaymentScheduleGuidAsync(Guid paymentScheduleGuid)
        {
            try
            {
                return await _context.paymentProofs
                    .Where(p => p.PaymentScheduleGuid == paymentScheduleGuid)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving payment proofs by PaymentScheduleGuid {paymentScheduleGuid}: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<PaymentProof> GetPaymentProofByIdAsync(int id)
        {
            return await _context.paymentProofs
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
