using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Persistence.MongoDb.Abstract;
using Persistence.MongoDb.Data;
using Persistence.MongoDb.Internal;

namespace Persistence.MongoDb.DependencyInjection;

public static class RegisterMongoDbModuleExtension
{
	public static IServiceCollection RegisterMongoDbModule<TDbContext>(this IServiceCollection services,
		Assembly assembly,
		Action<MongoContextConfiguration> contextConfiguration) where TDbContext : class, IMongoDbContext
	{
		services.AddScoped<IMongoDbContext, TDbContext>();
		services.AddScoped<TDbContext>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		services.AddMediatR(assembly);

		var configuration = new MongoContextConfiguration();
		contextConfiguration.Invoke(configuration);

		services.AddSingleton(configuration);


		return services;
	}

	public static void InitInternalAfterSetup(IServiceCollection services)
	{
		using var scope = services.BuildServiceProvider().CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<IMongoDbContext>();
		dbContext.InternalCreateIndexesAsync();
	}
}
