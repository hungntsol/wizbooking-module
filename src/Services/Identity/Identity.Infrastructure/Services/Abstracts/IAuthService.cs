using System.Security.Claims;

namespace Identity.Infrastructure.Services.Abstracts;
public interface IAuthService
{
    /// <summary>
    /// Generate Jwt access token from account
    /// </summary>
    /// <param name="userAccount"></param>
    /// <returns></returns>
    string GenerateAccessToken(UserAccount userAccount);

    /// <summary>
    /// Parse and retrieve all claim from Jwt token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    IList<Claim> RetrieveClaimFromJwtToken(string token);
}
