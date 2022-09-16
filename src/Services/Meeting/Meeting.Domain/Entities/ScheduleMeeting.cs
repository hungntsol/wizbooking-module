namespace Meeting.Domain.Entities;

[BsonCollection("scheduleMeeting")]
public class ScheduleMeeting : DocumentEntityBase<string>
{
	public ScheduleMeeting(
		string title,
		string? summary,
		ulong hostId,
		ulong guestId,
		DateTime startTime,
		TimeSpan lifeTime,
		string status)
	{
		Title = title;
		Summary = summary;
		HostId = hostId;
		GuestId = guestId;
		Status = status;
		Year = startTime.Year;
		Month = startTime.Month;
		Day = startTime.Day;
		Date = startTime.ToShortDateString();
		TimestampStart = BsonTimestamp.Create(startTime.Ticks);
		TimestampEnd = BsonTimestamp.Create(startTime.Add(lifeTime).Ticks);
	}

	public string Title { get; set; }
	public string? Summary { get; set; }
	public ulong HostId { get; set; }
	public ulong GuestId { get; set; }
	public string Status { get; set; }
	public int Year { get; set; }
	public int Month { get; set; }
	public int Day { get; set; }
	public string Date { get; set; }
	public BsonTimestamp TimestampStart { get; set; }
	public BsonTimestamp TimestampEnd { get; set; }

	public static ScheduleMeeting New(string title,
		string? summary,
		ulong hostId,
		ulong guestId,
		int minutes)
	{
		return new ScheduleMeeting(title,
			summary,
			hostId,
			guestId,
			DateTime.UtcNow,
			TimeSpan.FromMinutes(minutes),
			MeetingStatus.PENDING.Mapping());
	}

	/// <summary>
	/// Check as if this booking is valid at now
	/// </summary>
	/// <returns></returns>
	public bool IsValid()
	{
		var nowTick = BsonTimestamp.Create(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
		return TimestampStart < nowTick
		       && nowTick < TimestampEnd
		       && Status.Equals(MeetingStatus.ACCEPTED.Mapping());
	}

	/// <summary>
	/// Update the status of booking 
	/// Flow: PENDING => ACCEPT => REJECT
	/// Host can make CANCELED action at any stage
	/// </summary>
	/// <param name="updateStatus"></param>
	/// <returns></returns>
	public bool ChangeStatus(MeetingStatus updateStatus)
	{
		switch (updateStatus)
		{
			case MeetingStatus.ACCEPTED:
				if (!Status.Equals(MeetingStatus.PENDING.Mapping()))
				{
					return false;
				}

				break;

			case MeetingStatus.REJECTED:
				if (!Status.Equals(MeetingStatus.ACCEPTED.Mapping()))
				{
					return false;
				}

				break;

			case MeetingStatus.PENDING:
				if (!Status.Equals(MeetingStatus.CANCELED.Mapping()))
				{
					return false;
				}

				break;

			case MeetingStatus.CANCELED:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(updateStatus), updateStatus, null);
		}

		Status = updateStatus.Mapping();
		return true;
	}

	/// <summary>
	/// Update the datetime of meeting
	/// </summary>
	/// <param name="minutes"></param>
	public void ChangeDateTime(int minutes)
	{
		var now = DateTime.UtcNow;
		Year = now.Year;
		Month = now.Month;
		Day = now.Day;
		Date = now.ToShortDateString();
		TimestampStart = BsonTimestamp.Create(now.Ticks);
		TimestampEnd = BsonTimestamp.Create(now.AddMinutes(minutes).Ticks);
	}
}

public enum MeetingStatus
{
	PENDING,
	ACCEPTED,
	REJECTED,
	CANCELED
}

public static class MeetingStateExtension
{
	public static string Mapping(this MeetingStatus status)
	{
		return status switch
		{
			MeetingStatus.PENDING => nameof(MeetingStatus.PENDING),
			MeetingStatus.ACCEPTED => nameof(MeetingStatus.ACCEPTED),
			MeetingStatus.REJECTED => nameof(MeetingStatus.REJECTED),
			MeetingStatus.CANCELED => nameof(MeetingStatus.CANCELED),
			_ => throw new NotImplementedException()
		};
	}
}
