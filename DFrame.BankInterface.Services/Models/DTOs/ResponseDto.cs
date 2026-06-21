using ExpenseTracker.Services.Contracts.RequestResponseDto;
using Microsoft.AspNetCore.Http.Features;
using System.IO.Pipelines;

namespace ExpenseTracker.Services.Models.DTOs
{
    public class ResponseDto : IResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
