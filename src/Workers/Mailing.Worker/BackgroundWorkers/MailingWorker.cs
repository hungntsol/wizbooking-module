﻿using EventBusMessage.Abstracts;
using Mailing.Worker.Abstracts;
using SharedCommon.Commons.Logger;
using SharedCommon.Commons.Mailing.Models;

namespace Mailing.Worker.BackgroundWorkers;

internal class MailingWorker : IMessageConsumer<SendMailEventBusMessage>
{
    private readonly ILoggerAdapter<MailingWorker> _logger;
    private readonly IMaillingService _mailService;

    public MailingWorker(ILoggerAdapter<MailingWorker> logger, IMaillingService mailService)
    {
        _logger = logger;
        _mailService = mailService;
    }

    public async Task Consume(SendMailEventBusMessage message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Consume mail message id {@MessageId}", message.Id);

        await _mailService.SendEmailTemplateAsync<ConfirmAccountMailModel>(message);
    }
}