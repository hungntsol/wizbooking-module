using EventBusMessage.DependencyInjection;
using Mailing.Worker.Abstracts;
using Mailing.Worker.BackgroundWorkers;
using Mailing.Worker.Services;
using Mailing.Worker.SettingOptions;
using SharedEventBus.Events;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostCtx, services) =>
    {

        services.Configure<MailProviderAppSetting>(hostCtx.Configuration.GetSection("EmailSettings"));

        services.AddSingleton<IMailProviderConnection, MailProviderConnection>();
        services.AddSingleton<IMaillingService, MailingService>();

        // add rabbitMQ
        var rabbitmqSection = hostCtx.Configuration.GetSection("EventBus");
        services.AddRabbitMQ(
              settings =>
        {
            settings.HostAddress = rabbitmqSection.GetValue<string>("HostAddress");
            settings.ExchangeName = rabbitmqSection.GetValue<string>("ExchangeName");
            settings.ExchangeType = rabbitmqSection.GetValue<string>("ExchangeType");
            settings.QueuePrefetch = rabbitmqSection.GetValue<ushort>("QueuePrefetch");
            settings.RetryCount = rabbitmqSection.GetValue<int>("RetryCount");
        }, queue =>
        {
            queue.Add<SendMailEventBus>(default, "Worker.Mailing.Send");
        })
            .AddConsumer<SendMailEventBus, MailingWorker>();
    })
    .Build();

await host.RunAsync();
