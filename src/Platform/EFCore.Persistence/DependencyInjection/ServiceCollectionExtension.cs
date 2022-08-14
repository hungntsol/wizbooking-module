using System.Reflection;
using EFCore.Persistence.Abstracts;
using EFCore.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedCommon.RegisterModules;

namespace EFCore.Persistence.DependencyInjection;

public static class ServiceCollectionExtension
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
