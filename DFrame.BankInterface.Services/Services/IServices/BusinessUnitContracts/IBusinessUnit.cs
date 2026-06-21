using ExpenseTracker.Services.Contracts.RequestResponseDto;
using ExpenseTracker.Services.Models.DTOs;
namespace ExpenseTracker.Services.Contracts.IContracts.BusinessUnitContracts
{
    public interface IBusinessUnit
    {
        Task<ResponseDto> GetAllBU();
        Task<ResponseDto> GetBU(int buId);
        Task<ResponseDto> UpdateBU(BusinessUnitRequestDto businessUnitRequestDto);
        Task<ResponseDto> DeleteBU(int buId);

        Task<ResponseDto> CreateBusinessUnit(BusinessUnitRequestDto businessUnitRequestDto);
    }
}
