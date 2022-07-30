using Meeting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meeting.Domain.EntitiesConfigure;
internal sealed class AppUserServiceTypeConfigure : IEntityTypeConfiguration<AppUserService>
{
    public void Configure(EntityTypeBuilder<AppUserService> builder)
    {
        builder.HasKey(pk => new { pk.Id, pk.OwnerId, pk.Name });
    }
}
