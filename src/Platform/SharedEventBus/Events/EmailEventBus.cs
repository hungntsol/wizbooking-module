using EventBusMessage.Events.Base;
using Microsoft.AspNetCore.Http;

namespace SharedEventBus.Events;
public class SendMailEventBusMessage : IntegrationEventBase
{
    public string To { get; set; } = null!;
    public string? From { get; set; }
    public string Subject { get; set; } = null!;
    public string TemplateName { get; set; } = null!;
    public string? TemplateModel { get; set; }
    public IList<IFormFile>? Attachments { get; set; }

    public SendMailEventBusMessage()
    {
        
    }

    public SendMailEventBusMessage(string to, string from, string subject, string templateName, string templateModel, IList<IFormFile> attachments)
    {
        To = to;
        From = from;
        Subject = subject;
        TemplateName = templateName;
        TemplateModel = templateModel;
        Attachments = attachments;
    }

    public void AddAttachment(IFormFile attachment)
    {
        Attachments ??= new List<IFormFile>();
        Attachments.Add(attachment);
    }

    public void RemoveAttachment(IFormFile attachment)
    {
        Attachments?.Remove(attachment);
    }
}
