namespace ExpenseTracker.Services.Models.DTOs.Referrals
{
    public class ReferralsPerformanceDto
    {
        public int BDId { get; set; }
        public int? ClientId { get; set; }
        public string Name { get; set; }
        public string PanNo { get; set; }
        public string Address { get; set; }
        public DateTime JoiningDate { get; set; }
        public bool IsClient { get; set; }
        public bool Status { get; set; }
        public List<ReferalsClientsDetails> ClientsDetails { get; set; }
        public decimal TotalInvestmentBrought { get; set; }
        public int Rank { get; set; }
    }

    public class ReferalsClientsDetails
    {
        public int ClientId { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string? BloodRelation { get; set; }
        public string? RefferedByName { get; set; }
        public List<ReferalsClientsInvestments> InvestmentsDetails { get; set; }

    }

    public class ReferalsClientsInvestments
    {
        public int InvestmentId { get; set; }
        public int PlanId { get; set; }
        public string? PlanName { get; set; }
        public decimal InvestmentAmount { get; set; }
        public DateOnly InvestmentStartDate { get; set; }
        public DateOnly InvestmentEndDate { get; set; }
        public int Status { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string? ClosedBy { get; set; }
        public string? ClosingComment { get; set; }


    }
}
