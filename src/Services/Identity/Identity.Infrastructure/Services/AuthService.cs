using Identity.Infrastructure.Services.Abstracts;
using Identity.Infrastructure.SettingOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Infrastructure.Services;
public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuthAppSetting _authAppSetting;

    public AuthService(IHttpContextAccessor httpContextAccessor,
        IOptions<AuthAppSetting> authSettingOption)
    {
        _httpContextAccessor = httpContextAccessor;
        _authAppSetting = authSettingOption.Value;
    }

    public string GenerateAccessToken(UserAccount userAccount)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Email, userAccount.Email),
            new(ClaimTypes.Role, userAccount.Role)
        };

        // credential
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authAppSetting.SecretKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // describe token
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(_authAppSetting.ExpirationMinutes),
            SigningCredentials = credentials,
        };

        // generate token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public IList<Claim> RetrieveClaimFromJwtToken(string token)
    {
        throw new NotImplementedException();
    }
}
