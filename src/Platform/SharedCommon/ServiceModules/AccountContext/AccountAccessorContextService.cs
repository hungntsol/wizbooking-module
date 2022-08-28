using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using static System.UInt64;

namespace SharedCommon.ServiceModules.AccountContext;

public class AccountAccessorContextService : IAccountAccessorContextService
{
	private readonly ClaimsPrincipal _claimsPrincipal;

	public AccountAccessorContextService(IHttpContextAccessor httpContextAccessor)
	{
		_claimsPrincipal = httpContextAccessor.HttpContext!.User;
	}

	public ulong GetIdentifier()
	{
		var idStr = _claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
		if (TryParse(idStr, out var id))
		{
			return id;
		}

		throw new InvalidOperationException();
	}

	public string GetEmail()
	{
		return _claimsPrincipal.FindFirst(ClaimTypes.Email)!.Value;
	}

	public string Role()
	{
		return _claimsPrincipal.FindFirst(ClaimTypes.Role)!.Value;
	}
}
