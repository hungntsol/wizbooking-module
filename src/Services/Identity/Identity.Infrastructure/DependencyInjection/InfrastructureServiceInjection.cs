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
			var connectionString = configuration.GetValue<string>("DbContext:ConnectionString");
			options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30)),
				builder => builder.MigrationsAssembly("Identity.Infrastructure"));
		});

		services.AddRepositories();
		services.AddServices(configuration);
		services.AddAppSettingOptions(configuration);

		return services;
	}
}
