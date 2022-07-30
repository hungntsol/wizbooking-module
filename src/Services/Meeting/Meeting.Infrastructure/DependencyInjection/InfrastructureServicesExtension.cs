using EFCore.Persistence.DependencyInjection;
using Meeting.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Meeting.Infrastructure.DependencyInjection;
public static class InfrastructureServicesExtension
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, 
        ILogger logger,
        IConfiguration configuration)
    {
        services.AddDbContext<MeetingDataContext>(options =>
        {
            var connectionString = configuration.GetValue<string>("MeetingDataContext:ConnectionString");
            logger.LogInformation("MeetingDb's connectionString: {Connection}", connectionString);

            options.UseSqlServer(connectionString);
        });

        services.AddUnitOfWork<MeetingDataContext>();

        return services;
    }
}
