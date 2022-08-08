using Mailing.Worker.Abstracts;
using Mailing.Worker.Engine;
using Mailing.Worker.SettingOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using SharedCommon.Commons.Logger;

namespace Mailing.Worker.Services;

internal class MailingService : IMailingService
{
    private readonly IMailProviderConnection _providerConnection;
    private readonly ILoggerAdapter<MailingService> _logger;
    private readonly MailProviderAppSetting _mailProviderSetting;
    private readonly IViewEngineRenderer _viewEngineRenderer;

    public MailingService(IMailProviderConnection providerConnection,
        ILoggerAdapter<MailingService> logger,
        IOptions<MailProviderAppSetting> mailProviderSettingOption, IViewEngineRenderer viewEngineRenderer)
    {
        _providerConnection = providerConnection;
        _logger = logger;
        _viewEngineRenderer = viewEngineRenderer;
        _mailProviderSetting = mailProviderSettingOption.Value;
    }

    public async Task SendEmailAsync(string to, string? from, string subject, string htmlBody,
        IList<IFormFile>? attachments = null)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(string.IsNullOrEmpty(from) ? _mailProviderSetting.EmailFrom : from));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        // build htmlBody
        var bodyBuilder = new BodyBuilder();

        // attachments
        if (attachments is not null)
        {
            foreach (var file in attachments)
            {
                if (file.Length <= 0)
                {
                    continue;
                }

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                bodyBuilder.Attachments.Add(file.Name, fileBytes, ContentType.Parse(file.ContentType));
            }
        }

        // html
        bodyBuilder.HtmlBody = htmlBody;
        email.Body = bodyBuilder.ToMessageBody();

        if (!_providerConnection.IsConnected)
        {
            _providerConnection.TryConnect();
        }

        try
        {
            await _providerConnection.SmtpClient().SendAsync(email);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "FATAL ERROR: Cannot send email\n {Message}", e.Message);

            _providerConnection.TryConnect();

            throw;
        }
    }

    public async Task SendEmailTemplateAsync<TModel>(SendMailEventBusMessage @event)
    {
        var html = await _viewEngineRenderer.RenderAsStringAsync<TModel>(@event.TemplateName, @event.TemplateModel);
        await this.SendEmailAsync(
            @event.To,
            @event.From,
            @event.Subject,
            html,
            @event.Attachments);
    }
}