using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.ClientMaster;

namespace ExpenseTracker.Services.Services.IServices
{
    public interface IClientMasterService
	{
		Task<ResponseDto> GetAllClientMastersAsync();
		Task<ResponseDto> GetClientMasterByIdAsync(int id);
		Task<ResponseDto> GetClientMasterByGuidAsync(Guid id);
		Task<ResponseDto> AddClientMasterAsync(ClientMasterRequestDto clientMaster);
		Task<ResponseDto> UpdateClientMasterAsync(ClientMasterRequestDto clientMaster);
		Task<ResponseDto> DeleteClientMasterAsync(int id);
		Task<ResponseDto> GetBankingDetailsByClientIdAsync(int clientId);
    }
}
