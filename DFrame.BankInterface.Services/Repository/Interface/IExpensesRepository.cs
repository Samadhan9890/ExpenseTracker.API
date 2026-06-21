using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Repository.Interface
{
    public interface IExpensesRepository
    {
        Task<List<Expense>> GetExpensesAsync();
    }
}