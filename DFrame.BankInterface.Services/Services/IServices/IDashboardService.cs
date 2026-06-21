using ExpenseTracker.Services.Models.DTOs;

namespace ExpenseTracker.Services.Services.IServices
{
	public interface IDashboardService
	{
		ResponseDto GetDashboardData(string type, DateOnly? periodFrom, DateOnly? periodTo);
	}
}
