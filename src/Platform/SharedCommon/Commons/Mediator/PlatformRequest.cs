namespace SharedCommon.Commons.Mediator;

public interface IPlatformRequest
{
	Guid GetAuditedTrackedId();
}

public class PlatformRequest : IPlatformRequest
{
	protected Guid HandleAuditedTrackId { get; private set; }
	protected DateTime HandleAuditedDateTime { get; private set; }
	protected string? HandleAuditedByUserId { get; private set; }

	public Guid GetAuditedTrackedId()
	{
		return HandleAuditedTrackId;
	}

	public IPlatformRequest PopulateAuditInfo(
		Guid handleAuditedTrackId,
		DateTime handleAuditedDateTime,
		string? handleAuditedByUserId)
	{
		HandleAuditedTrackId = handleAuditedTrackId;
		HandleAuditedByUserId = handleAuditedByUserId;
		HandleAuditedDateTime = handleAuditedDateTime;
		return this;
	}
}
