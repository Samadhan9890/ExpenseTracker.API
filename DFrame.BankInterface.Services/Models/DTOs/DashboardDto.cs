namespace ExpenseTracker.Services.Models.DTOs
{
	public class DashboardDto
	{
        public decimal Ium { get; set; }
		public decimal Tim { get; set; }
		public decimal ProfitPaid { get; set; }
		public decimal todaysPayments { get; set; }
		public int TotalClients { get; set; }

        public decimal PaymentForecast { get; set; }


        public List<InvestmentMonthwiseSummary> InvMonthwiseSummary { get; set; }
		public List<PaymentsMonthiwseSummary> PaymentMonthwiseSummary { get; set; }
		public List<PaymentsMonthiwseSummary> UpcommingPayments { get; set; }

		public List<PlansSummary> PlansSummary { get; set; }
		public List<BusinessDevTeamPerformance> BusinessDevTeamPerformance { get; set; }


	}

	public class InvestmentMonthwiseSummary
	{
        public string MonthName { get; set; }
		public int YearNum { get; set; }
		public decimal TotalInvestment { get; set; }
	}

	public class PaymentsMonthiwseSummary
	{
		public string MonthName { get; set; }
		public int YearNum { get; set; }
		public decimal AmountPaid { get; set; }
	}

	public class PlansSummary
	{
        public string PLAN_NAME { get; set; }
        public decimal INVESTMENT_AMOUNT { get; set; }
        public int NO_OF_INVESTMENTS { get; set; }
    }

	public class BusinessDevTeamPerformance
	{
        public string BusinessDevTeamMember { get; set; }
		public int TotalClients { get; set; }
        public decimal TotalInvestment { get; set; }
    }
}
