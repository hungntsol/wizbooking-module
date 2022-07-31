﻿namespace Mailing.Worker.SettingOptions;
internal class MailProviderAppSetting
{
	public string Host { get; set; } = null!;
	public ushort Port { get; set; }
	public string Username { get; set; } = null!;
	public string Password { get; set; } = null!;
	public string? EmailFrom { get; set; }
}
