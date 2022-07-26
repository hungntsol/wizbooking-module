﻿using EventBusMessage.Abstracts;
using EventBusMessage.Events.Base;
using EventBusMessage.RabbitMQ.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;

namespace EventBusMessage.RabbitMQ;
public class EventBusQueueListener<T> : BackgroundService where T : IntegrationEventBase
{
    private readonly IMessageProducer _messageProducer;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventBusQueueListener<T>> _logger;
    private readonly RabbitMQManagerSettings _rabbitMQManagerSettings;
    private readonly IEventBusPersistence _rabbitMQPersistence;
    private readonly string _queueName;

    public EventBusQueueListener(
        IMessageProducer messagePublisher,
        IServiceProvider serviceProvider,
        ILogger<EventBusQueueListener<T>> logger,
        RabbitMQManagerSettings rabbitMQManagerSettings,
        EventBusQueueManager queuesManager,
        IEventBusPersistence rabbitMQPersistence)
    {
        _messageProducer = messagePublisher;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _rabbitMQManagerSettings = rabbitMQManagerSettings;
        _rabbitMQPersistence = rabbitMQPersistence;

        _queueName = queuesManager.Queues.First(q => q.type == typeof(T)).name;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        if (!_rabbitMQPersistence.IsConnected)
        {
            _rabbitMQPersistence.TryConnect();
        }

        var consumer = new EventingBasicConsumer(_messageProducer.GetChannel());
        consumer.Received += async (sender, args) =>
        {
            try
            {
                var message = Encoding.UTF8.GetString(args.Body.Span);

                _logger.LogInformation("Received message: {Message}", message);

                using var scope = _serviceProvider.CreateScope();
                var receiver = scope.ServiceProvider.GetRequiredService<IMessageConsumer<T>>();
                var eventMessage = JsonSerializer.Deserialize<T>(message, _rabbitMQManagerSettings.JsonSerializerOptions);

                if (eventMessage is not null)
                {
                    await receiver.Consume(eventMessage, stoppingToken);
                    _messageProducer.MarkAsComplete(args);
                    _logger.LogInformation("Message processed {MessageId}: {Message}", eventMessage!.Id, message);
                }
                else
                {
                    _messageProducer.MarkAsRejected(args);
                    _logger.LogError("Message is null {MessageId}: {Message}", eventMessage!.Id, message);
                }
            }
            catch (Exception e)
            {
                _messageProducer.MarkAsRejected(args);
                _logger.LogError(e, "Error processing message {MessageId}", args.BasicProperties.MessageId);
            }

            stoppingToken.ThrowIfCancellationRequested();
        };

        _messageProducer.GetChannel().BasicConsume(_queueName, false, consumer);

        return Task.CompletedTask;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting RabbitMQ listener for {QueueName}", _queueName);
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping RabbitMQ listener for {QueueName}", _queueName);
        return base.StopAsync(cancellationToken);
    }
}
