using Meeting.Infrastructure.DependencyInjection;
using Meeting.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Meeting.Api.ServicesExtension;

public static class ApplicationConfigureExtension
{
    public static IServiceCollection Setup(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddInfrastructureService(configuration);

        return services;
    }

    public static WebApplication Configure(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MigrateAppDataContext();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    /// <summary>
	/// Migrate database to latest version if it is not yet updated.
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	private static WebApplication MigrateAppDataContext(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<MeetingDataContext>();

        if (!dataContext.Database.CanConnect())
        {
            dataContext.Database.Migrate();
        }

        return app;
    }
}
