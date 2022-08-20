using Microsoft.Extensions.DependencyInjection;
using Persistence.MongoDb.Abstract;
using Persistence.MongoDb.Data;
using Persistence.MongoDb.Internal;

namespace Persistence.MongoDb.DependencyInjection;

public static class RegisterMongoDbModuleExtension
{
	public static IServiceCollection RegisterMongoDbModule(this IServiceCollection services,
		Action<MongoContextConfiguration> contextConfiguration)
	{
		services.AddScoped<IMongoDbContext, MongoDbContext>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		var configuration = new MongoContextConfiguration();
		contextConfiguration.Invoke(configuration);

		services.AddSingleton(configuration);

		return services;
	}
}
