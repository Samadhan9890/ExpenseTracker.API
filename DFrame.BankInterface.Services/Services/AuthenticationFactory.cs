using ExpenseTracker.Services.Contracts.IContracts;
using ExpenseTracker.Services.Contracts.IContracts.JWTContracts;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using OnlyOtp;
using ExpenseTracker.Services.Models.ServiceModels;
using ExpenseTracker.Services.Services.IServices;

namespace ExpenseTracker.Services.Contracts
{
    public class AuthenticationFactory : IAuthenticationFactory
    {
        private readonly AppDBContext _dbContext;
        private readonly IJwtUtils _jwtUtils;
        private readonly IConfiguration _config;
        private readonly IMessageService _messageService;
		public AuthenticationFactory(AppDBContext appDBContext, IJwtUtils jwtUtils,IConfiguration config,IMessageService messageService) {
            _dbContext = appDBContext;
            _jwtUtils = jwtUtils;
            _config = config;
            _messageService = messageService;
        }  
        public async Task<string> AuthenticateUser(AuthenticationRequestDto authenticationRequestDto)
        {
            string userId = authenticationRequestDto.userId;
            string pass = authenticationRequestDto.password;            
            TblUser user = await _dbContext.Users.Where(x => x.UserLoginCode == userId ).SingleOrDefaultAsync();

            if(user== null)
            {
                throw new Exception("User not found");
            }

            if (user.IsLocked == true)
            {
                throw new Exception("User is locked. Please contact your administrator.");
            }

            if (user.Status == 0)
            {
                throw new Exception("User is inactive. Please contact your administrator.");
            }

            if (!BCrypt.Net.BCrypt.Verify(pass, user.UserPassword)) {

                if(user != null)
                {
                    int failedAttempts = user.NoOfAttempt??0;
                    user.NoOfAttempt = failedAttempts+1;

                    if(user.NoOfAttempt > 3)
                    {
                        user.IsLocked = true;
                    }

                    _dbContext.SaveChanges();
                }
                throw new Exception("Incorrect user name or password. Too many failed attempts will lock the account.");
            }
            
            user.IsLocked = false;
            _dbContext.SaveChanges();


            string Token =  _jwtUtils.GetJwtToken(user);

            if (user.IS_MFA??false)
            {
				var otpProvider = new Otp();
                var otp = otpProvider.GenerateOtp();
                user.LAST_OTP =  int.Parse(otp);
                user.OtpExpiryDate = DateTime.Now.AddMinutes(5);

                if(user.MobileNo == null)
                {
                    throw new Exception("Mobile Number not available to send OTP.");
                }
				await SenOtpOnWhatsapp(user);
                await _dbContext.SaveChangesAsync();
			}
			return Token;
        }

		public async Task<ResponseDto> ValidateOtp(int otp,UserClaims user)
		{
			var userDetails = await _dbContext.Users.Where(u => u.InternalUserId == user.InternalUserId).FirstOrDefaultAsync();
            ResponseDto response = new ResponseDto();
            if(userDetails!= null && userDetails.LAST_OTP == otp && userDetails.OtpExpiryDate >= DateTime.Now)
            {
                response.Result= true;
            }
            else
            {
                response.IsSuccess= false;
                response.Result = "Invalid / Expired Otp";
            }
            return response;

		}

		private async Task  SenOtpOnWhatsapp(TblUser user)
        {
            try
            {
                string message = $"Hello {user.UserName}," +
                    $"Please user {user.LAST_OTP} to login into portal." +
                    $"If its not you please contact your administrator immidiately.";				 
                await _messageService.SendMessage(message, user.MobileNo);               
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to Send OTP via Twilio Whatsapp.");
            }		
		}


    }
}
