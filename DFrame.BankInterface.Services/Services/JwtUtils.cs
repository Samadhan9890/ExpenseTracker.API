//*** System Define Classes and Namespace ***//
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

//*** User Define Classes and Namespace ***//
using ExpenseTracker.Services.Contracts.IContracts.JWTContracts;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Utilities.ConstantHelper;


namespace ExpenseTracker.Services.Contracts
{
    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _configuration;
        
        public JwtUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string JWT_SECRET { 
            get {
                return _configuration.GetSection(ConstantHelper.JWT_SECRET).Value ?? string.Empty;
            } 
        }

        public string JWT_ISSUER { 
            get
            {
                return _configuration.GetSection(ConstantHelper.JWT_ISSUER).Value ?? string.Empty;
            }
         }
        public string GetJwtToken(TblUser user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // Get the key and Encript it.
            byte[] key = Encoding.UTF8.GetBytes(JWT_SECRET);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            //Create the signing key using encrypted key and Algorithms
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            //Create Cliams
            ClaimsIdentity claimsIdentity = GetClaimsIdentity(user);
            //Create Token Descriptor and add token details
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor();
            tokenDescriptor.Subject = claimsIdentity;
            tokenDescriptor.Issuer = JWT_ISSUER;
            tokenDescriptor.Expires = DateTime.UtcNow.AddMinutes(240);
            tokenDescriptor.SigningCredentials = signingCredentials;
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);           
        }

        private ClaimsIdentity GetClaimsIdentity(TblUser user)
        {
            ClaimsIdentity identity = new();
            Claim[] claims = {
                new Claim("InternalUserId", user.InternalUserId.ToString() ?? "0"),
                new Claim("UserLoginCode", user?.UserLoginCode?.ToString() ?? ""),
                new Claim("UserName", user?.UserName?.ToString() ?? ""),
                new Claim("EmployeeCode", user?.EmployeeCode?.ToString() ?? ""),
                new Claim("RoleAccess" , user?.RoleAccess?.ToString() ?? ""),
				new Claim("IsMfa" , user?.IS_MFA.ToString() ?? "false"),
			};

            identity.AddClaims(claims);
            return identity;
        }
    }
}
