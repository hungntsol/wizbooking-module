using EventBusMessage.Abstracts;
using Mailing.Worker.Abstracts;

namespace Mailing.Worker.BackgroundWorkers;

internal class MailingWorker : IMessageConsumer<SendMailEventBus>
{
    private readonly ILogger<MailingWorker> _logger;
    private readonly IMaillingService _mailService;

    public MailingWorker(ILogger<MailingWorker> logger, IMaillingService mailService)
    {
        _logger = logger;
        _mailService = mailService;
    }

    public async Task Consume(SendMailEventBus message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Consume mail message id {@MessageId}", message.Id);

        await _mailService.SendEmailTemplateAsync(message);
    }
}