using EventBusMessage.Abstracts;
using Mailing.Worker.Abstracts;
using SharedCommon.Commons.Mailing;
using SharedCommon.Commons.Mailing.Models;

namespace Mailing.Worker.BackgroundWorkers;

internal class SendingMailWorker : IMessageConsumer<SendMailEventBusMessage>
{
    private readonly ILoggerAdapter<SendingMailWorker> _logger;
    private readonly IMailingService _mailService;

    public SendingMailWorker(ILoggerAdapter<SendingMailWorker> logger, IMailingService mailService)
    {
        _logger = logger;
        _mailService = mailService;
    }

    public async Task Consume(SendMailEventBusMessage message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Consume email for ConfirmAccount: message {@MessageId}", message.Id);

        switch (message.TemplateName)
        {
            case EmailTemplateConstants.ConfirmAccountMail:
                await _mailService.SendEmailTemplateAsync<ConfirmAccountMailModel>(message);
                break;

            case EmailTemplateConstants.ResetAccountMail:
                await _mailService.SendEmailTemplateAsync<ResetAccountMailModel>(message);
                break;

            default:
                _logger.LogError("Not found any template like {TemplateName}", message.TemplateName);
                break;
        }
    }
}
