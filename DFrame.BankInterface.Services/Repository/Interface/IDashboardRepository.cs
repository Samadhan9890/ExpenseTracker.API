using System.Data;

namespace ExpenseTracker.Services.Repository.Interface
{
	public interface IDashboardRepository
	{
		DataSet GetDashboardData(string type,DateOnly? periodFrom,DateOnly? periodTo);
	}
}
