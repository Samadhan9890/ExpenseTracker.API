using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Services.IServices;
using System.Net;

namespace ExpenseTracker.Services.Controllers
{
    [Route("api/expenses")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpensesService _expensesService;
        private ResponseDto _responseDto;

        public ExpensesController(
            IExpensesService expensesService,
            ResponseDto responseDto)
        {
            _expensesService = expensesService;
            _responseDto = responseDto;
        }

        [HttpGet]
        [Route("GetExpenses")]
        public async Task<IActionResult> GetExpenses()
        {
            _responseDto = await _expensesService.GetExpensesAsync();

            return StatusCode((int)HttpStatusCode.OK, _responseDto);
        }
    }
}

//using ExpenseTracker.Services.Services.IServices;
//using Microsoft.AspNetCore.Mvc;

//namespace ExpenseTracker.Services.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ExpensesController : ControllerBase
//    {
//        private readonly IExpensesService _expensesService;

//        public ExpensesController(IExpensesService expensesService)
//        {
//            _expensesService = expensesService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetExpenses()
//        {
//            var expenses = await _expensesService.GetExpensesAsync();

//            return Ok(expenses);
//        }
//    }
//}