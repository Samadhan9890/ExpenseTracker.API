using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;

namespace ExpenseTracker.Services.Services
{
    public class ExpensesService : IExpensesService
    {
        private readonly IExpensesRepository _expensesRepository;
        private readonly ResponseDto _responseDto;

        public ExpensesService(
            IExpensesRepository expensesRepository,
            ResponseDto responseDto)
        {
            _expensesRepository = expensesRepository;
            _responseDto = responseDto;
        }

        public async Task<ResponseDto> GetExpensesAsync()
        {
            try
            {
                var expenses = await _expensesRepository.GetExpensesAsync();

                _responseDto.IsSuccess = true;
                _responseDto.Message = "Expenses retrieved successfully.";
                _responseDto.Result = expenses;

                return _responseDto;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

                return _responseDto;
            }
        }
    }
}