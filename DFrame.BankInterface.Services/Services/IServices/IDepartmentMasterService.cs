using ExpenseTracker.Services.Models.DTOs;

namespace ExpenseTracker.Services.Services.IServices
{
    public interface IDepartmentMasterService
    {
        Task<ResponseDto> GetAllDepartmentMaster();
    }
}
