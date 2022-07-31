using EventBusMessage.Abstracts;
using Mailing.Worker.Abstracts;
using Mailing.Worker.SettingOptions;
using Microsoft.Extensions.Options;
using SharedEventBus.Events;

namespace Mailing.Worker.BackgroundWorkers;
internal class MailingWorker : IMessageConsumer<SendMailEventBus>
{
    private readonly ILogger<MailingWorker> _logger;
    private readonly IMaillingService _mailService;
    private readonly MailProviderAppSetting _mailProviderSetting;

    public MailingWorker(ILogger<MailingWorker> logger, IMaillingService mailService, IOptions<MailProviderAppSetting> mailProviderSettingOptions)
    {
        _logger = logger;
        _mailService = mailService;
        _mailProviderSetting = mailProviderSettingOptions.Value;
    }

    public async Task SubscribeAsync(SendMailEventBus message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Consumer mail message: {@Message}", message);

        await _mailService.SendEmailAsync(message.To, _mailProviderSetting.EmailFrom!, message.Subject,
            "Hello from WizBooking Test");
    }
}
