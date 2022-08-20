using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.EfCore.Abstracts;
using Persistence.EfCore.Data;
using SharedCommon.RegisterModules;

namespace Persistence.EfCore.DependencyInjection;

public static class RegisterEfCoreModuleExtension
{
	public static IServiceCollection RegisterEfCoreModule<TContext>(this IServiceCollection services, Assembly assembly)
		where TContext : DbContext
	{
		services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
		services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
		services.AddMediatR(assembly);
		services.RegisterMapping();

		return services;
	}
}
