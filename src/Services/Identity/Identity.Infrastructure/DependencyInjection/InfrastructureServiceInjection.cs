using EFCore.Persistence.DependencyInjection;
using Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.DependencyInjection;

public static class InfrastructureServiceInjection
{
    public static IServiceCollection InjectInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDataContext>(options =>
        {
            options.UseSqlServer(configuration.GetValue<string>("DbContext:ConnectionString"),
                action => action.MigrationsAssembly("Identity.Infrastructure"));
        });

        services.AddUnitOfWork<IdentityDataContext>();

        services.AddRepositories();
        services.AddServices(configuration);
        services.AddAppSettingOptions(configuration);

        return services;
    }
}