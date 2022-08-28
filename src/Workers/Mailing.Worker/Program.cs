using EventBusMessage.DependencyInjection;
using Mailing.Worker.Abstracts;
using Mailing.Worker.BackgroundWorkers;
using Mailing.Worker.Engine;
using Mailing.Worker.Services;
using Mailing.Worker.SettingOptions;
using RazorLight;
using SharedCommon.RegisterModules;

var host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostCtx, services) =>
	{
		services.RegisterLoggerAdapter();

		services.Configure<MailProviderAppSetting>(hostCtx.Configuration.GetSection("EmailSettings"));

		services.AddSingleton<IMailProviderConnection, MailProviderConnection>();
		services.AddSingleton<IMailingService, MailingService>();

		services.AddSingleton<IViewEngineRenderer, ViewEngineRenderer>();
		services.AddSingleton(new RazorLightEngineBuilder()
			.UseMemoryCachingProvider()
			.UseFileSystemProject(Directory.GetCurrentDirectory())
			.EnableDebugMode()
			.Build());

		services.AddRabbitMQ((setting, queue) =>
			{
				hostCtx.Configuration.GetSection("EventBus").Bind(setting);
				queue.Add<SendMailEventBusMessage>(default, "Worker.Mailing.Send");
			})
			.AddConsumer<SendMailEventBusMessage, SendingMailWorker>();
	})
	.Build();

await host.RunAsync();
