namespace ExpenseTracker.Services.Contracts.RequestResponseDto
{
    public interface IResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
