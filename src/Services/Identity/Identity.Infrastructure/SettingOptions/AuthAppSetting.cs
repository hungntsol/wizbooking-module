namespace Identity.Infrastructure.SettingOptions;

/// <summary>
/// Auth properties load from appsetting
/// </summary>
public class AuthAppSetting
{
    public bool EnableVerifiedEmail { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string SecretKey { get; set; } = null!;
    public int ExpirationMinutes { get; set; }
    public ushort ConfirmLinkExpiredMinutes { get; set; }
    public ushort ResetLinkExpiredMinutes { get; set; }
}