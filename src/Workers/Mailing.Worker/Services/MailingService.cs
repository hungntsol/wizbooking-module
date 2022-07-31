using Mailing.Worker.Abstracts;
using Mailing.Worker.SettingOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using SharedEventBus.Events;

namespace Mailing.Worker.Services;
internal class MailingService : IMaillingService
{
    private readonly IMailProviderConnection _providerConnection;
    private readonly ILogger<MailingService> _logger;
    private readonly MailProviderAppSetting _mailProviderSetting;

    public MailingService(IMailProviderConnection providerConnection, 
        ILogger<MailingService> logger,
        IOptions<MailProviderAppSetting> mailProviderSettingOption)
    {
        _providerConnection = providerConnection;
        _logger = logger;
        _mailProviderSetting = mailProviderSettingOption.Value;
    }

    public async Task SendEmailAsync(string to, string from, string subject, string htmlBody, IList<IFormFile>? attachments = null)
    {
		var email = new MimeMessage();
		email.From.Add(MailboxAddress.Parse(_mailProviderSetting.EmailFrom ?? from));
		email.To.Add(MailboxAddress.Parse(to));
		email.Subject = subject;

		var bodyBuilder = new BodyBuilder();
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
			_logger.LogCritical(e, "FATAL ERROR: CANNOT SEND EMAIL\n {Message}", e.Message);
			throw;
		}
	}

    public Task SendEmailTemplateAsync(SendMailEventBus @event)
    {
        throw new NotImplementedException();
    }
}
