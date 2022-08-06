using EventBusMessage.DependencyInjection;
using Mailing.Worker.Abstracts;
using Mailing.Worker.BackgroundWorkers;
using Mailing.Worker.Engine;
using Mailing.Worker.Services;
using Mailing.Worker.SettingOptions;
using RazorLight;
using RazorLight.Extensions;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostCtx, services) =>
    {
        services.Configure<MailProviderAppSetting>(hostCtx.Configuration.GetSection("EmailSettings"));

        services.AddSingleton<IMailProviderConnection, MailProviderConnection>();
        services.AddSingleton<IMaillingService, MailingService>();

        services.AddSingleton<IViewEngineRenderer, ViewEngineRenderer>();
        services.AddSingleton<RazorLightEngine>(new RazorLightEngineBuilder()
            .UseMemoryCachingProvider()
            .UseFileSystemProject(Directory.GetCurrentDirectory())
            .EnableDebugMode()
            .Build());

        services.AddRabbitMQ((setting, queue) =>
            {
                var section = hostCtx.Configuration.GetSection("EventBus");
                section.Bind(setting);
                queue.Add<SendMailEventBus>(default, "Worker.Mailing.Send");
            })
            .AddConsumer<SendMailEventBus, MailingWorker>();
    })
    .Build();

await host.RunAsync();