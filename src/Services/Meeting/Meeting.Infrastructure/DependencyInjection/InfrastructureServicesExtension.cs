using EFCore.Persistence.DependencyInjection;
using Meeting.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meeting.Infrastructure.DependencyInjection;
public static class InfrastructureServicesExtension
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MeetingDataContext>(options => 
            options.UseSqlServer(configuration.GetValue<string>("MeetingDataContext:ConnectionString"))
        );

        services.AddUnitOfWork<MeetingDataContext>();

        return services;
    }
}
