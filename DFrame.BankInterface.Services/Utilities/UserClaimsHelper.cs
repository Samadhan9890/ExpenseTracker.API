using ExpenseTracker.Services.Contracts;
using ExpenseTracker.Services.Models.ServiceModels;
using System.Security.Claims;

namespace ExpenseTracker.Services.Utilities
{
	public class UserClaimsHelper
	{
		/// <summary>
		/// Get the user details from token i.e. identity model and created an user object.
		/// </summary>
		public static UserClaims GetClaims(ClaimsIdentity claimsIdentity)
		{
			if (claimsIdentity != null)
			{
				IEnumerable<Claim> claims = claimsIdentity.Claims;
				UserClaims userDetails = new UserClaims();
				if (claims.Any(c => c.Type == "InternalUserId"))
				{
					userDetails.InternalUserId = Convert.ToInt32(claims.Where(c => c.Type == "InternalUserId").FirstOrDefault().Value);
				}
				else
				{
					throw new Exception("Invalid claim in token : InternalUserId");
				}
				if (claims.Any(c => c.Type == "UserLoginCode"))
				{
					userDetails.UserLoginCode = claims.Where(c => c.Type == "UserLoginCode").FirstOrDefault().Value;
				}
				else
				{	
					throw new Exception("Invalid claim in token : UserLoginCode");
				}
				if (claims.Any(c => c.Type == "UserName"))
				{
					userDetails.UserName = claims.Where(c => c.Type == "UserName").FirstOrDefault().Value;
				}
				else
				{
					throw new Exception("Invalid claim in token : UserName");
				}
				if (claims.Any(c => c.Type == "EmployeeCode"))
				{
					userDetails.EmployeeCode = claims.Where(c => c.Type == "EmployeeCode").FirstOrDefault().Value;
				}
				else
				{
					throw new Exception("Invalid claim in token : EmployeeCode");
				}
				if (claims.Any(c => c.Type == "RoleAccess"))
				{
					userDetails.RoleAccess = claims.Where(c => c.Type == "RoleAccess").FirstOrDefault().Value;
				}
				else
				{
					throw new Exception("Invalid claim in token : RoleAccess");
				}
				
				//if (claims.Any(c => c.Type == "RoleId"))
				//{
				//	userDetails.RoleId = Convert.ToInt32(claims.Where(c => c.Type == "RoleId").FirstOrDefault().Value);
				//}
				//else
				//{
				//	throw new Exception("Invalid claim in token : RoleId");
				//}
				

				return userDetails;
			}
			else
			{
				throw new Exception("The claims are null.");
			}
		}
	}
}
