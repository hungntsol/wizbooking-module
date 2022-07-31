using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Application.DependencyInjection;
public static class ApplicationServiceInjection
{
    public static IServiceCollection InjectApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}
