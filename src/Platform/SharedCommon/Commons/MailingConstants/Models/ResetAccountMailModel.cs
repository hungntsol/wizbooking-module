namespace SharedCommon.Commons.MailingConstants.Models;

public class ResetAccountMailModel
{
	public ResetAccountMailModel(string confirmUrl)
	{
		ConfirmUrl = confirmUrl;
	}

	public string ConfirmUrl { get; init; }
}
