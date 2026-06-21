using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Services.Contracts;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Repository.Interface;
using System.Data;

namespace ExpenseTracker.Services.Repository
{
	public class DashboardRepository : IDashboardRepository
	{
		private readonly AppDBContext _dbContext;
        public DashboardRepository(AppDBContext appDBContext)
        {
            _dbContext = appDBContext;
        }
        public DataSet GetDashboardData(string type, DateOnly? periodFrom, DateOnly? periodTo)
		{
			Dictionary<string, string> spParams = new Dictionary<string, string>();
			spParams.Add("@COMMAND", type);
			spParams.Add("@PERIOD_FROM", periodFrom == null ? null : periodFrom.ToString());
			spParams.Add("@PERIOD_TO", periodTo == null ? null : periodTo.ToString());


			SqlAccess sqlAccess = new SqlAccess(_dbContext);
			var dsReult = sqlAccess.ExecuteMultiScalarSp("USP_GET_DASHBOARD_DATA", spParams);
			return dsReult;

		}
	}
}
