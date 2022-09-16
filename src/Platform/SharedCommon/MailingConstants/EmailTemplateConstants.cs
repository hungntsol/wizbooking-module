namespace SharedCommon.MailingConstants;

public static class EmailTemplateConstants
{
	private const string RootPathTemplates = "Views/";

	public const string ConfirmAccountMail = RootPathTemplates + "ConfirmAccountMail.cshtml";
	public const string ResetAccountMail = RootPathTemplates + "ResetAccountMail.cshtml";
}
