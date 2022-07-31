using Microsoft.EntityFrameworkCore;
using SharedCommon.Domain;

namespace Meeting.Domain.Entities;

[Index(nameof(Name), Name = "IX_UserServiceName", IsUnique = false)]
public class AppUserService : EntityBase<ulong>
{
    public ulong OwnerId { get; set; }
    public virtual AppUser Owner { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public AppUserService(ulong ownerId, string name, string? description, bool isActive)
    {
        OwnerId = ownerId;
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    public bool IsValid() => this.IsActive;
}
