using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;

namespace ExpenseTracker.Services.Repository
{
    public class ExpensesRepository : IExpensesRepository
    {
        private readonly AppDBContext _appDbContext;

        public ExpensesRepository(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Expense>> GetExpensesAsync()
        {
            return await _appDbContext.Expenses
                .AsNoTracking()
                .OrderByDescending(x => x.DateSpent)
                .ToListAsync();
        }
    }
}