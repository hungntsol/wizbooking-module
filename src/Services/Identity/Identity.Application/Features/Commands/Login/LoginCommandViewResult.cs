using Identity.Application.Features.Queries.Profile;

namespace Identity.Application.Features.Commands.Login;
public class LoginCommandViewResult
{
    public string AccessToken { get; set; }
    public GetProfileResultView Profile { get; set; }

    public LoginCommandViewResult(string accessToken, GetProfileResultView profile)
    {
        AccessToken = accessToken;
        Profile = profile;
    }
}
