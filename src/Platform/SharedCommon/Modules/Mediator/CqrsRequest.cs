namespace SharedCommon.Modules.Mediator;

public interface ICqrsRequest
{
	Guid GetAuditedTrackedId();
}

public class CqrsRequest : ICqrsRequest
{
	protected Guid HandleAuditedTrackId { get; private set; }
	protected DateTime HandleAuditedDateTime { get; private set; }
	protected string? HandleAuditedByUserId { get; private set; }

	public Guid GetAuditedTrackedId()
	{
		return HandleAuditedTrackId;
	}

	public ICqrsRequest PopulateAuditInfo(
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

public interface ICqrsRequestHandler<TRequest> where TRequest : ICqrsRequest
{
}
