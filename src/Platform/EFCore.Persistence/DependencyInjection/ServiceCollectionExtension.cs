using System.Reflection;
using EFCore.Persistence.Abstracts;
using EFCore.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Persistence.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services, Assembly assembly)
        where TContext : DbContext
    {
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();

        return services;
    }
}
