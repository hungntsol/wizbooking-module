using Microsoft.AspNetCore.Http;

namespace Mailing.Worker.Abstracts;

internal interface IMaillingService
{
    /// <summary>
    /// Send email async with rawHtmlBody
    /// </summary>
    /// <param name="to"></param>
    /// <param name="from"></param>
    /// <param name="subject"></param>
    /// <param name="htmlBody"></param>
    /// <param name="attachments"></param>
    /// <returns></returns>
    Task SendEmailAsync(string to, string? from, string subject, string htmlBody, IList<IFormFile>? attachments = null);

    /// <summary>
    /// Resolve email template and send email
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    Task SendEmailTemplateAsync<TModel>(SendMailEventBusMessage @event);
}