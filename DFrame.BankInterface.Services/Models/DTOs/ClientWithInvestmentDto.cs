using ExpenseTracker.Services.Models.DTOs.Subscriptions;

namespace ExpenseTracker.Services.Models.DTOs
{
    public class ClientWithInvestmentDto
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string AadharNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

        public List<InvestmentsDto> Investments { get; set; }
    }
}
