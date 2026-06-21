using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Repository.Interface
{
    public interface IPaymentProofRepository
    {
        Task<PaymentProof> AddpaymentProofAsync(PaymentProof paymentProof);
        Task<List<PaymentProof>> GetPaymentProofByPaymentScheduleGuidAsync(Guid paymentScheduleGuid);
        Task<PaymentProof> GetPaymentProofByIdAsync(int id);
    }
}
