using ExpenseTracker.Services.Contracts.IContracts.JWTContracts;
using ExpenseTracker.Services.Utilities.ConstantHelper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ExpenseTracker.Services.StartUpServices
{
    public static class WebAppBuilderExtensionMethods
    {
		/// <summary>
		/// This is the extension method of webapplication builder
		/// Here the return type should be webapp builder and paramer should accept 1st arg
		/// as webapp builder with this keyword
		/// This extension adds Authorization settings/defaults
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
		{
			//Authorization settings
			var secrete = builder.Configuration.GetValue<string>("JWT:SECRET");
			var issuer = builder.Configuration.GetValue<string>("JWT:ISSUER");			
			var key = Encoding.UTF8.GetBytes(secrete);
			//configuration for authorization
			builder.Services.AddAuthentication(x =>
			{
				//1. Set auth and challenge scheme
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			 .AddJwtBearer(x =>
			 {
				 x.TokenValidationParameters = new TokenValidationParameters()
				 {
					 ValidateIssuerSigningKey = true,
					 IssuerSigningKey = new SymmetricSecurityKey(key),
					 ValidateIssuer = true,
					 ValidIssuer = issuer,
					 ValidateAudience = false
				 };
			 });

			return builder;
		}
	}
}
