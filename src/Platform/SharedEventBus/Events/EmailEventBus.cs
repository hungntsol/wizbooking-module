using EventBusMessage.Events.Base;
using Microsoft.AspNetCore.Http;

namespace SharedEventBus.Events;
public class SendMailEventBus : IntegrationEventBase
{
	public string To { get; set; } = null!;
	public string Subject { get; set; } = null!;
	public string TemplateName { get; set; } = null!;
	public object? TemplateModel { get; set; }
	public string? DisplayName { get; set; }

	public IList<IFormFile>? Attachments { get; set; }

	public void AddAttachment(IFormFile attachment)
	{
		if (Attachments is null)
			Attachments = new List<IFormFile>();
		Attachments.Add(attachment);
	}

	public void RemoveAttachment(IFormFile attachment)
	{
		Attachments?.Remove(attachment);
	}
}
