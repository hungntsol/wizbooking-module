using Microsoft.EntityFrameworkCore;
using Persistence.EfCore.Internal;

namespace Identity.Domain.Entities;

[Index(nameof(AppCode), Name = "IX_Appcode", IsUnique = false)]
public class VerifiedUrl : EntityBase<Guid>
{
	public VerifiedUrl(string email, string target, DateTime expiredAt)
	{
		AppCode = Guid.NewGuid().ToString();
		Email = email;
		ExpiredAt = expiredAt;
		Target = target;
	}

	public string AppCode { get; set; }
	public string Email { get; set; }
	public string Target { get; set; }
	public DateTime ExpiredAt { get; set; }

	public static VerifiedUrl New(string email, string target, TimeSpan span)
	{
		return new VerifiedUrl(
			email,
			target,
			DateTime.UtcNow.AddMinutes(span.Minutes));
	}

	public bool IsValid()
	{
		return ExpiredAt > DateTime.UtcNow;
	}
}
