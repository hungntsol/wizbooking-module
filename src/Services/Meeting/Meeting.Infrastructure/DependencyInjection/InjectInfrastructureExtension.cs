using Meeting.Infrastructure.Persistence.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.MongoDb.DependencyInjection;

namespace Meeting.Infrastructure.DependencyInjection;

public static class InjectInfrastructureExtension
{
	/// <summary>
	/// Inject all dependency in Infrastructure layer
	/// </summary>
	/// <param name="services"></param>
	/// <param name="configuration"></param>
	/// <returns></returns>
	public static IServiceCollection InjectInfrastructureLayer(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.InjectRepositories();
		services.InjectService();

		ConfigureMappingSource();
		InjectModuleExtension.InitInternalAfterSetup(services);

		return services;
	}

	/// <summary>
	/// Config mapping document in mongo
	/// </summary>
	private static void ConfigureMappingSource()
	{
		ScheduleMeetingEntityMappingSource.Configure();
	}

	/// <summary>
	/// Inject all repositories in Infrastructure layer
	/// </summary>
	/// <param name="services"></param>
	private static void InjectRepositories(this IServiceCollection services)
	{
	}

	private static void InjectService(this IServiceCollection services)
	{
	}
}
