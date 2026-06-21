using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.DTOs
{
    public class ClientBankingDetailsDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int InvestmentId { get; set; }
        public Guid InvestmentGuid { get; set; }

        [Required(ErrorMessage = "Mode is required.")]
        public string Mode { get; set; }

        [Required(ErrorMessage = "AccountNoOrUpiId is required.")]
        public string AccountNoOrUpiId { get; set; }
        public string? BankName { get; set; }
        public string? AccountHolderName { get; set; }
        public string? IFSCCode { get; set; }
        public bool Status { get; set; } = true;

        public DateTime CreatedDate { get; set; } 

        [Required(ErrorMessage = "CreatedBy is required.")]
        public string CreatedBy { get; set; }

        // New property to store the type of banking details ("Referral" or "Client")
        public string? BankingType { get; set; }
        public int BusinessDevTeamId { get; set; }
        public string? Note { get; set; }

    }

}
