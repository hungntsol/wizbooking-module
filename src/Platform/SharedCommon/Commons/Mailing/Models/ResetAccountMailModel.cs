namespace SharedCommon.Commons.Mailing.Models;

public class ResetAccountMailModel
{
    public string ConfirmUrl { get; init; } = null!;

    public ResetAccountMailModel(string confirmUrl)
    {
        ConfirmUrl = confirmUrl;
    }
}