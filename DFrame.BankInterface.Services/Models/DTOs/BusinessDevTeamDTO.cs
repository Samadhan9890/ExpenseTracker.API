namespace ExpenseTracker.Services.Models.DTOs
{
    public class BusinessDevTeamDTO
    {
        public int BDId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PanNo { get; set; }
        public string AadharNo { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime EntryDate { get; set; }
        public bool Status { get; set; }

        // New property to hold banking details
        public List<ClientBankingDetailsDto>? BankingDetails { get; set; } = new List<ClientBankingDetailsDto>();

    }
}
