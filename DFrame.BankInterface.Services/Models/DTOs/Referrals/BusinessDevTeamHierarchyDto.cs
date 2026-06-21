namespace ExpenseTracker.Services.Models.DTOs.Referrals
{
    public class ClientHierarchyDto
    {
        public ClientDto Client { get; set; }
        public ClientDto ParentClient { get; set; }
        public List<ClientDto> ChildClients { get; set; }
    }

    public class ClientDto
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public List<InvestmentDto> Investments { get; set; } = new List<InvestmentDto>();
    }

    public class InvestmentDto
    {
        public int InvestmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime InvestmentDate { get; set; }
    }


}
