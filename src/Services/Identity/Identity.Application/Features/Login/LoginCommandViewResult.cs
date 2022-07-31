namespace Identity.Application.Features.Login;
public class LoginCommandViewResult
{
    public string AccessToken { get; set; }
    public UserAccount Account { get; set; }

    public LoginCommandViewResult(string accessToken, UserAccount account)
    {
        AccessToken = accessToken;
        Account = account;
    }
}
