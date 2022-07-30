using Microsoft.EntityFrameworkCore;
using SharedCommon.Domain;

namespace Meeting.Domain.Entities;

[Index(nameof(Url), Name = "IX_ProviderUrl", IsUnique = true)]
public class ProvidedUrl : EntityBase<ulong>
{
    public string Url { get; set; } = null!;
    public ulong CreatorId { get; set; }
    public virtual AppUser Creator { get; set; } = null!;
    public DateTime ExpiredAt { get; set; }
    public string AllowServicesPattern { get; set; } = "*";
    public string? DenyServicesPattern { get; set; }
    public bool IsActive { get; set; }

    public ProvidedUrl(string url,
        ulong creatorId,
        DateTime expiredAt,
        string allowServicesPattern,
        string? denyServicesPattern,
        bool isActive)
    {
        Url = url;
        CreatorId = creatorId;
        ExpiredAt = expiredAt;
        AllowServicesPattern = allowServicesPattern;
        DenyServicesPattern = denyServicesPattern;
        IsActive = isActive;
    }

    public bool IsValid()
    {
        return this.ExpiredAt < DateTime.UtcNow;
    }
}
