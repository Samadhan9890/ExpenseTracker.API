using ExpenseTracker.Services.Models.DTOs;

namespace ExpenseTracker.Services.Services.IServices
{
    public interface IExpensesService
    {
        Task<ResponseDto> GetExpensesAsync();
    }
}