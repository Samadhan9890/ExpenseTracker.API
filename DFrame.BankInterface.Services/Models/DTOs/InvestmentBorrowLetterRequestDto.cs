namespace ExpenseTracker.Services.Models.DTOs
{
    public class InvestmentBorrowLetterRequestDto
    {
        public int InvestmentId { get; set; } 

        public string? BorrowLetter { get; set; } 

        public int BorrowLetterStatus { get; set; } 
    }
}
