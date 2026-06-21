using ExpenseTracker.Services.Models.DTOs;

namespace ExpenseTracker.Services.Services.IServices
{
    public interface ILocationMasterService
    {
        Task<ResponseDto> GetAllLocationMaster();
    }
}
