namespace Mailing.Worker.MailTemplates;

public class ConfirmAccountMailModel
{
    public string ConfirmUrl { get; set; }
    public string ResendUrl { get; set; }

    public ConfirmAccountMailModel(string confirmUrl, string resendUrl)
    {
        ConfirmUrl = confirmUrl;
        ResendUrl = resendUrl;
    }
}