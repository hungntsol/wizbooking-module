using EventMessageBus.Abstracts;
using EventMessageBus.Events.Base;
using EventMessageBus.Options;
using EventMessageBus.RabbitMQ;
using EventMessageBus.RabbitMQ.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace EventMessageBus.DependencyInjection;
public static class EventBusMessageServiceExtension
{
	public static IMessageBuilder AddRabbitMQ(
		this IServiceCollection services,
		Action<RabbitMQManagerSettings> rabbitMqConfigure,
		Action<RabbitMQQueueManager> queueConfigure)
	{
		var rabbitMqSettings = new RabbitMQManagerSettings();
		rabbitMqConfigure.Invoke(rabbitMqSettings);

		var queueManager = new RabbitMQQueueManager();
		queueConfigure.Invoke(queueManager);


		services.AddSingleton(rabbitMqSettings);
		services.AddSingleton(queueManager);

		services.AddSingleton<IRabbitMQPersistence, RabbitMQPersistence>();
		services.AddSingleton<IMessageProducer, RabbitMQMessageManager>();

		return new DefaultMessageBuilder(services);
	}

	public static IMessageBuilder AddConsumer<TObject, TConsumer>(this IMessageBuilder builder)
		where TObject : IntegrationEventBase
		where TConsumer : class, IMessageConsumer<TObject>
	{
		builder.ServiceCollection.AddHostedService<RabbitMQQueueListener<TObject>>();
		builder.ServiceCollection.AddScoped<IMessageConsumer<TObject>, TConsumer>();

		return builder;
	}
}
