using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedCommon.Commons.Exceptions;
using SharedCommon.Modules.JwtAuth.AccountContext;

namespace SharedCommon.Modules.JwtAuth;

public static class JwtAuthInjection
{
	/// <summary>
	/// Register authentication for service
	/// </summary>
	/// <param name="services"></param>
	/// <param name="configuration"></param>
	/// <returns></returns>
	public static IServiceCollection AddJwtAuthModule(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		services.AddTransient<IAccountAccessorContextService, AccountAccessorContextService>();
		services.AddAuthorization();

		var authSection = configuration.GetSection("AuthSettings");
		var signingKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(authSection.GetValue<string>("SecretKey")));

		NullReferenceObjectException.ThrowIfNull(signingKey, nameof(signingKey));

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = signingKey,
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});

		return services;
	}
}
