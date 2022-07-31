using Microsoft.EntityFrameworkCore;
using SharedCommon.Domain;

namespace Identity.Domain.Entities;

[Index(nameof(AppCode), Name = "IX_Appcode", IsUnique = false)]
public class VerifiedUrl : EntityBase<Guid>
{
    public string AppCode { get; set; }
    public string Email { get; set; }
    public string Target { get; set; }
    public DateTime ExpiredAt { get; set; }

    public VerifiedUrl(string appCode, string email, string target, DateTime expiredAt)
    {
        AppCode = appCode;
        Email = email;
        ExpiredAt = expiredAt;
        Target = target;
    }
}
