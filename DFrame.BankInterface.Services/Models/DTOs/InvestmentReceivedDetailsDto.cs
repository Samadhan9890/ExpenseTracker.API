using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.DTOs
{
    public class InvestmentReceivedDetailDto
    {
        public int Id { get; set; }
        public int ProcessId { get; set; } = 0;
        public int InvestmentId { get; set; }
        public Guid InvestmentGuid { get; set; }


        [Required(ErrorMessage = "Mode is required.")]
        public string Mode { get; set; }
        public string? BankName { get; set; }

        [Required(ErrorMessage = "AccountNumberOrUpiId is required.")]
        public string? AccountNumberOrUpiId { get; set; }
        [Range(typeof(decimal), "0.01", "9999999999", ErrorMessage = "InvestmentAmount must be greater than 0.")]
        public decimal Amount { get; set; }
        public string? Comments { get; set; }

        [Required(ErrorMessage = "AddedBy is required.")]
        public string AddedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateOnly ReceivedDate { get; set; }
        public bool Status { get; set; }
        public byte[]? InvestmentAttachment { get; set; }
    }

}
