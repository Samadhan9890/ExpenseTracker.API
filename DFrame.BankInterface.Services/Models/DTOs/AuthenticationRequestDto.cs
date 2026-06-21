using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.DTOs
{
    public class AuthenticationRequestDto
    {
        //[RegularExpression("[a-zA-Z]")]
        public string userId { get; set; }

        //Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character:
        //[RegularExpression("\"^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$\"")]
        public string password { get; set; }
    }
}
