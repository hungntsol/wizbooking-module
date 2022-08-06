namespace SharedCommon.Commons.Mailing.Models;

public class ConfirmAccountMailModel
{
    public string ConfirmUrl { get; set; } = null!;
    public string ResendUrl { get; set; } = null!;
}