using Microsoft.EntityFrameworkCore;
using SharedCommon.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Meeting.Domain.Entities;

[Index(nameof(Title), Name = "IX_TITLE", IsUnique = false)]
public class Booking : EntityBase<ulong>
{
    public string Title { get; set; } = null!;
    public string? Summary { get; set; }
    public ulong HostId { get; set; }
    [ForeignKey("HostId")]
    public virtual AppUser Host { get; set; } = null!;
    public ulong GuestId { get; set; }
    [ForeignKey("GuestId")]
    public virtual AppUser Guest { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }

    public Booking(string title, string? summary, ulong hostId, ulong guestId, DateTime startTime, DateTime endTime, string status)
    {
        Title = title;
        Summary = summary;
        HostId = hostId;
        GuestId = guestId;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
    }

    /// <summary>
    /// Check as if this booking is valid at now
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        var now = DateTime.UtcNow;
        return (this.StartTime < now) && (now < this.EndTime);
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
                if (!this.Status.Equals(MeetingStatus.PENDING.Mapping()))
                    return false;
                break;

            case MeetingStatus.REJECTED:
                if (!this.Status.Equals(MeetingStatus.ACCEPTED.Mapping()))
                    return false;
                break;

            case MeetingStatus.PENDING:
                if (!this.Status.Equals(MeetingStatus.CANCELED.Mapping()))
                    return false;
                break;

            case MeetingStatus.CANCELED:
                break;
        }

        this.Status = updateStatus.Mapping();
        return true;
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
        => status switch
        {
            MeetingStatus.PENDING => nameof(MeetingStatus.PENDING),
            MeetingStatus.ACCEPTED => nameof(MeetingStatus.ACCEPTED),
            MeetingStatus.REJECTED => nameof(MeetingStatus.REJECTED),
            MeetingStatus.CANCELED => nameof(MeetingStatus.CANCELED),
            _ => throw new NotImplementedException(),
        };
}
