using EventBusMessage.Abstracts;
using EventBusMessage.Events.Base;
using EventBusMessage.Options;
using EventBusMessage.RabbitMQ;
using EventBusMessage.RabbitMQ.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventBusMessage.DependencyInjection;
public static class EventBusMessageServiceExtension
{
    public static IMessageBuilder AddRabbitMQ(
        this IServiceCollection services,
        Action<RabbitMQManagerSettings, EventBusQueueManager> config)
    {
        var rabbitMqSettings = new RabbitMQManagerSettings();
        var queueManager = new EventBusQueueManager();
        config.Invoke(rabbitMqSettings, queueManager);

        services.AddSingleton(rabbitMqSettings);
        services.AddSingleton(queueManager);

        services.AddSingleton<IEventBusPersistence, EventBusPersistence>();
        services.AddSingleton<IMessageProducer, EventBusManager>();

        return new DefaultMessageBuilder(services);
    }

    public static IMessageBuilder AddConsumer<TObject, TConsumer>(this IMessageBuilder builder)
        where TObject : IntegrationEventBase
        where TConsumer : class, IMessageConsumer<TObject>
    {
        builder.ServiceCollection.AddHostedService<EventBusQueueListener<TObject>>();
        builder.ServiceCollection.AddScoped<IMessageConsumer<TObject>, TConsumer>();

        return builder;
    }
}
