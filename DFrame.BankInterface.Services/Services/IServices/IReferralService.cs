using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.Referrals;

namespace ExpenseTracker.Services.Services.IServices
{
    public interface IReferralService
    {
        Task<ResponseDto> GetAllBdPerformance();
        Task<ResponseDto> GetBdHierarchybyClientId(int clientId);

    }
}
