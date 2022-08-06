using Identity.Infrastructure.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Identity.Infrastructure.Services;
public class AccountAccessorService : IAccountAccessorService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountAccessorService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetEmail()
    {
        return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
    }

    public string Role()
    {
        return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
    }
}
