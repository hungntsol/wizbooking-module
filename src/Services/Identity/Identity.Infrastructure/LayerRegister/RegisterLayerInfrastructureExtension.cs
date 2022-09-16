using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Repositories.Abstracts;
using Identity.Infrastructure.Services;
using Identity.Infrastructure.Services.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.LayerRegister;

public static class RegisterLayerInfrastructureExtension
{
	public static IServiceCollection RegisterLayerInfrastructure(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<AppDbContext>(options =>
		{
			var connectionString = configuration.GetValue<string>("DbContext:ConnectionString");
			options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30)),
				builder => builder.MigrationsAssembly("Identity.Infrastructure"));
		});

		services.AddRepositories();
		services.AddServices(configuration);
		services.LoadInfraAppSettingOptions(configuration);

		return services;
	}

	private static void AddServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddTransient<IAuthService, AuthService>();
	}

	private static void AddRepositories(this IServiceCollection services)
	{
		services.AddTransient<IUserAccountRepository, UserAccountRepository>();
		services.AddTransient<IVerifiedUrlRepository, VerifiedUrlRepository>();
	}
}
