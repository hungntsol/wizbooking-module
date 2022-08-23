namespace Meeting.Domain.Entities;

[BsonCollection("scheduleInviteUrl")]
public class ScheduleInviteUrl : DocumentBase<string>
{
	public ScheduleInviteUrl(string generatedUrl,
		ulong creatorId,
		DateTime expiredAt,
		IEnumerable<string>? allowServices,
		IEnumerable<string>? denyServices,
		bool isActive)
	{
		GeneratedUrl = generatedUrl;
		CreatorId = creatorId;
		ExpiredAt = expiredAt;
		AllowServices = allowServices ?? ScheduleInviteUrlConstant.All;
		DenyServices = denyServices;
		IsActive = isActive;
	}

	public string GeneratedUrl { get; set; }
	public ulong CreatorId { get; set; }
	public DateTime ExpiredAt { get; set; }
	public IEnumerable<string> AllowServices { get; set; }
	public IEnumerable<string>? DenyServices { get; set; }
	public bool IsActive { get; set; }

	public bool IsValid()
	{
		return ExpiredAt < DateTime.UtcNow && IsActive;
	}

	public void Deactivate()
	{
		IsActive = false;
	}

	public void Activate()
	{
		IsActive = true;
	}
}

public static class ScheduleInviteUrlConstant
{
	public const string AllowAll = "*";
	public static List<string> All = new() { AllowAll };
}
