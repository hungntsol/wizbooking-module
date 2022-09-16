using EventBusMessage.DependencyInjection;
using EventBusMessage.Events;

namespace Identity.Api.ServiceExtensions;

internal static class EventBusExtension
{
	internal static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddRabbitMQ((setting, queue) =>
		{
			configuration.GetSection("EventBus").Bind(setting);
			queue.Add<SendMailEventBusMessage>(default, "Worker.Mailing.Send");
		});


		return services;
	}
}
