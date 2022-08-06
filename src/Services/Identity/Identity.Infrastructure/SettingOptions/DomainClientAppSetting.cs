namespace Identity.Infrastructure.SettingOptions;

public class DomainClientAppSetting
{
    public string HttpProtocol { get; set; } = null!;
    public string Host { get; set; } = null!;
    public ushort Port { get; set; }

    public string Url()
    {
        return this.HttpProtocol + "://" + this.Host + ":" + this.Port;
    }
}