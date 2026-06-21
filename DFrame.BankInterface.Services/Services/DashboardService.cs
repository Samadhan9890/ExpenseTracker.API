using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;
using ExpenseTracker.Services.Utilities;

namespace ExpenseTracker.Services.Services
{
	public class DashboardService : IDashboardService
	{
		private readonly IDashboardRepository _dashRepository;
		private ResponseDto _responseDto;
		private readonly ILogger<DashboardService> _logger;

		private Dictionary<string, int> monthNamesToNumbers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
																{
																{"January", 1}, {"February", 2}, {"March", 3}, {"April", 4},
																{"May", 5}, {"June", 6}, {"July", 7}, {"August", 8},
																{"September", 9}, {"October", 10}, {"November", 11}, {"December", 12}
																};

		public DashboardService(IDashboardRepository dashRepository,ResponseDto responseDto,ILogger<DashboardService> logger)
        {
            _dashRepository = dashRepository;
			_responseDto = responseDto;
			_logger = logger;
        }
        public ResponseDto GetDashboardData(string type, DateOnly? periodFrom, DateOnly? periodTo)
		{
			try
			{
				var result = _dashRepository.GetDashboardData(type, periodFrom, periodTo);

				DashboardDto dashboardDto = new DashboardDto();
				dashboardDto.Ium = Convert.ToDecimal(result.Tables[0].Rows[0]["IUM"] ?? 0);
				dashboardDto.Tim = Convert.ToDecimal(result.Tables[0].Rows[0]["TIM"] ?? 0);
				dashboardDto.ProfitPaid = Convert.ToDecimal(result.Tables[0].Rows[0]["PROFIT_PAID"] ?? 0);
				dashboardDto.todaysPayments = Convert.ToDecimal(result.Tables[0].Rows[0]["TODAYS_PAYMENTS"] ?? 0);
				dashboardDto.TotalClients = Convert.ToInt32(result.Tables[0].Rows[0]["TOTAL_CLIENTS"] ?? 0);

				List<InvestmentMonthwiseSummary> investmentMonthwise = DatatableHelper.BindList<InvestmentMonthwiseSummary>(result.Tables[1]);
				dashboardDto.InvMonthwiseSummary = new();
				dashboardDto.InvMonthwiseSummary = investmentMonthwise;

				List<PaymentsMonthiwseSummary> paymentMonthwise = DatatableHelper.BindList<PaymentsMonthiwseSummary>(result.Tables[2]);
				dashboardDto.PaymentMonthwiseSummary = new();
				dashboardDto.PaymentMonthwiseSummary = paymentMonthwise;

				int currentMonth = DateTime.Now.Month;

				var upcommingPayments = paymentMonthwise
							.Where(p => p.YearNum >= DateTime.Now.Year &&
										monthNamesToNumbers[p.MonthName] > currentMonth)
							.ToList();

				dashboardDto.UpcommingPayments = new();
				dashboardDto.UpcommingPayments =upcommingPayments;

				dashboardDto.PaymentForecast = upcommingPayments.Sum(s => s.AmountPaid);

				List<PlansSummary> plansSummaries = DatatableHelper.BindList<PlansSummary>(result.Tables[3]);
				dashboardDto.PlansSummary = new();
				dashboardDto.PlansSummary = plansSummaries;

				List<BusinessDevTeamPerformance> businessDevTeamPerf = DatatableHelper.BindList<BusinessDevTeamPerformance>(result.Tables[4]);
				dashboardDto.BusinessDevTeamPerformance = new();
				dashboardDto.BusinessDevTeamPerformance = businessDevTeamPerf;



				_responseDto.Result = dashboardDto;
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.ToString();
				_logger.LogError($"Unable to get dashboard data : " + ex);
				
			}
			return _responseDto;
			

		}
	}
}
