using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Repositories.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.DependencyInjection;

internal static class RepositoriesInjection
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserAccountCoreRepository, UserAccountRepository>();
        services.AddTransient<IVerifiedUrlRepository, VerifiedUrlRepository>();

        return services;
    }
}