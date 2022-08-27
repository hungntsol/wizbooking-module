using Meeting.Infrastructure.DependencyInjection;
using SharedCommon.Commons.LoggerAdapter;

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

		services.RegisterLoggerAdapter();

		services.InjectInfrastructure(configuration);

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

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();
		return app;
	}
}
