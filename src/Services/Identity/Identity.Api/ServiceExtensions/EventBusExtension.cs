using EventBusMessage.DependencyInjection;
using SharedEventBus.Events;

namespace Identity.Api.ServiceExtensions;

internal static class EventBusExtension
{
    internal static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
		services.AddRabbitMQ((setting, queue) =>
        {
            configuration.GetSection("EventBus").Bind(setting);
            queue.Add<SendMailEventBusMessage>(default, "WorkerMailing.Send");
        });


		return services;
    }
}
