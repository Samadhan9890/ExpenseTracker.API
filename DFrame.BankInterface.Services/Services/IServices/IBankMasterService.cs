using ExpenseTracker.Services.Models.DTOs;

namespace ExpenseTracker.Services.Contracts.IContracts
{
	public interface IBankMasterService
	{
		Task<ResponseDto> GetAllBankMastersAsync();
		Task<ResponseDto> GetBankMasterByIdAsync(int id);

		Task<ResponseDto> AddBankMasterAsync(BankMasterDto bank);
		Task<ResponseDto> UpdateBankMasterAsync(BankMasterDto bank);

		Task<ResponseDto> DeleteBankMasterAsync(int id);

	}
}
