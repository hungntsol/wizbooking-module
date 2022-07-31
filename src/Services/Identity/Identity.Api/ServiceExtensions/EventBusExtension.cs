using EventBusMessage.DependencyInjection;
using SharedEventBus.Events;

namespace Identity.Api.ServiceExtensions;

internal static class EventBusExtension
{
    internal static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
		services.AddRabbitMQ(settings =>
		{
			var section = configuration.GetSection("EventBus");
			settings.HostAddress = section.GetValue<string>("HostAddress");
			settings.ExchangeName = section.GetValue<string>("ExchangeName");
			settings.ExchangeType = section.GetValue<string>("ExchangeType");
			settings.QueuePrefetch = section.GetValue<ushort>("QueuePrefetch");
			settings.RetryCount = section.GetValue<int>("RetryCount");
		}, queue =>
		{
			queue.Add<SendMailEventBus>(default, "Worker.Mailing.Send");
		});


		return services;
    }
}
