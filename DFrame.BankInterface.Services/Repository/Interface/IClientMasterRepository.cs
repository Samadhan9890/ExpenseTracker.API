using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Repository.Interface
{
	public interface IClientMasterRepository
	{
		Task<ClientMaster> GetClientMasterByIdAsync(int clientId);
		Task<ClientMaster> GetClientMasterByGuidAsync(Guid clientId);

		Task<List<ClientMaster>> GetAllClientMastersAsync();
		Task<ClientMaster> AddClientMasterAsync(ClientMaster clientMaster);
		Task UpdateClientMasterAsync(ClientMaster clientMaster);
		Task DeleteClientMasterAsync(int clientId);
		Task SaveChangesAsync();
		Task<List<ClientBankingDetail>> GetBankingDetailsByClientIdAsync(int clientId);

		Task CreateBankingDetails(List<ClientBankingDetail> bankingDetails);
    }
}
