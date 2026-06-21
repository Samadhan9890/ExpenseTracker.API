using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Services.IServices
{
    public interface IPaymentProofService
    {
        Task<ResponseDto> CreatePaymentProofAsync(PaymentProofDto paymentProofDto);
        Task<List<PaymentProof>> GetPaymentProofsByPaymentScheduleGuidAsync(Guid paymentScheduleGuid);
        Task<PaymentProof> GetPaymentProofByIdAsync(int id);
    }
}
