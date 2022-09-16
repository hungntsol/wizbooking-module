using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.EfCore.Abstracts;
using Persistence.EfCore.Data;
using SharedCommon.Modules.Mapping;

namespace Persistence.EfCore.DependencyInjection;

public static class InjectModuleExtension
{
	/// <summary>
	/// Already add MediaR and Mapping module
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
	/// <param name="services"></param>
	/// <param name="assembly"></param>
	/// <returns></returns>
	public static IServiceCollection AddEfCoreDbContext<TContext>(this IServiceCollection services, Assembly assembly)
		where TContext : DbContext
	{
		services.AddScoped<IEfCoreAtomicWork, EfCoreAtomicWork<TContext>>();
		services.AddScoped<IEfCoreAtomicWork<TContext>, EfCoreAtomicWork<TContext>>();
		services.AddMediatR(assembly);
		services.AddMapping();

		return services;
	}
}
