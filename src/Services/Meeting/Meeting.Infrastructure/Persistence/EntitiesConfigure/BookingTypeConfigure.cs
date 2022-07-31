using Meeting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meeting.Infrastructure.Persistence.EntitiesConfigure;
internal sealed class BookingTypeConfigure : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
		builder
			.HasOne<AppUser>(q => q.Guest)
			.WithMany(q => q.MeetingsWithHost)
			.HasForeignKey(q => q.GuestId)
			.OnDelete(DeleteBehavior.NoAction);

		builder
			.HasOne<AppUser>(q => q.Host)
			.WithMany(q => q.MeetingsWithGuest)
			.HasForeignKey(q => q.HostId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
