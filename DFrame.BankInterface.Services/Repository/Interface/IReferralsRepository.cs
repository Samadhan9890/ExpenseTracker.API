using ExpenseTracker.Services.Models.DTOs.Referrals;

namespace ExpenseTracker.Services.Repository.Interface
{
    public interface IReferralsRepository
    {
        Task<List<ReferralsPerformanceDto>> GetAllBdPerformance();

        Task<ClientHierarchyDto> GetAllBusinessDevTeamHierarchyByClientId(int clientId);

        Task<List<ClientHierarchyDto>> BusinessDevTeamHierarchies();
    }
}
