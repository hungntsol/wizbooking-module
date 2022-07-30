using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Meeting.Domain.Entities;

[Index(nameof(Title), Name = "IX_TITLE", IsUnique = false)]
public class Booking
{
    public string Title { get; set; } = null!;
    public string? Summary { get; set; }
    public int HostId { get; set; }
    [ForeignKey("HostId")]
    public virtual AppUser Host { get; set; } = null!;
    public int GuestId { get; set; }
    [ForeignKey("GuestId")]
    public virtual AppUser Guest { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }

    public Booking(string title, string? summary, int hostId, int guestId, DateTime startTime, DateTime endTime, MeetingStatus status)
    {
        Title = title;
        Summary = summary;
        HostId = hostId;
        GuestId = guestId;
        StartTime = startTime;
        EndTime = endTime;
        Status = status.Mapping();
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
