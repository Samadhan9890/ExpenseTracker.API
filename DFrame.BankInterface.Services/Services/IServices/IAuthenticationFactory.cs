using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.ServiceModels;

namespace ExpenseTracker.Services.Contracts.IContracts
{
    public interface IAuthenticationFactory
    {
         Task<string> AuthenticateUser(AuthenticationRequestDto authenticationRequestDto);
		Task<ResponseDto> ValidateOtp(int otp,UserClaims user);

	}
}
