using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Identity.Api.ServiceExtensions;

internal static class AuthenticationExtension
{
    internal static IServiceCollection InjectAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authSection = configuration.GetSection("AuthSettings");
        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(authSection.GetValue<string>("SecretKey")));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

        services.AddAuthorization();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
}
