using Identity.Infrastructure.Services;
using Identity.Infrastructure.Services.Abstracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.DependencyInjection;
internal static class ServicesInjection
{
    internal static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAuthService, AuthService>();

        return services;
    }
}
