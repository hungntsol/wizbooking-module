using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SharedCommon.ServiceModules.AccountContext;

public class AccountAccessorContextService : IAccountAccessorContextService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public AccountAccessorContextService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string GetEmail()
	{
		return _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Email)!.Value;
	}

	public string Role()
	{
		return _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Role)!.Value;
	}
}
