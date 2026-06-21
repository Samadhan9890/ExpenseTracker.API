using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.Dashboard;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;
using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Repository.Interface
{
	public interface IBusinessDevTeamRepository
    {

        Task<List<BusinessDevTeam>> GetAllBusineesDevTeamAsync();
        Task<List<ClientMaster>> GetAllBDTeamAsync();
        Task<BusinessDevTeam> GetBusineesDevTeamMemberByIdAsync(int BusineesDevTeamMemberId);
        Task<BusinessDevTeam> CreateBusinessDevTeamAsync(BusinessDevTeam addBusinessDevTeam);
        Task UpdateBusinessDevTeamAsync(BusinessDevTeam updateBusinessDevTeam);
        Task<ClientMaster> GetBusinessDevByClientId(int clientId);
        Task CreateBusinessDevTeamBankingDetailsAsync(List<ClientBankingDetail> bankingDetails);
        Task<ClientBankingDetail> UpdateBusinessDevTeamBankingDetailAsync(ClientBankingDetailsDto bankingDetailDto);
        Task<List<ClientBankingDetail>> GetBankingDetailsByBusinessDevTeamIdAsync(int bdId);
        Task<bool> CheckIfAadharOrPanExistsAsync(string aadharNumber, string panNumber);
    }
}
