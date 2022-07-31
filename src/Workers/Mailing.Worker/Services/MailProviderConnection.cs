using Mailing.Worker.Abstracts;
using Mailing.Worker.SettingOptions;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Polly;
using System.Net.Sockets;

namespace Mailing.Worker.Services;
internal class MailProviderConnection : IMailProviderConnection
{
	private readonly MailProviderAppSetting _emailProviderSetting;
	private readonly ILogger<MailProviderConnection> _logger;

	private ISmtpClient? _smtpClient;
	private readonly object _locker = new();

	public MailProviderConnection(IOptions<MailProviderAppSetting> mailProviderSettingOptions, ILogger<MailProviderConnection> logger)
	{
		_emailProviderSetting = mailProviderSettingOptions.Value;
		_logger = logger;

		InitiateConnection();
	}

	public ISmtpClient SmtpClient()
	{
		if (_smtpClient is null)
		{
			_logger.LogWarning("SmtpClient is null, trying to reconnect");

			if (TryConnect()) return _smtpClient!;

			_logger.LogCritical("Can't connect to smtp server");
			throw new Exception();
		}

		return _smtpClient!;
	}

	public bool IsConnected => _smtpClient is not null && _smtpClient.IsConnected && _smtpClient.IsAuthenticated;

	public bool TryConnect()
	{
		_logger.LogWarning("Attempt to connect to smtp server");

		lock (_locker)
		{
			var policy = Policy.Handle<SocketException>()
				.Or<Exception>()
				.WaitAndRetry(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), (ex, time) =>
				{
					_logger.LogWarning("Error while connecting to smtp server after {Timeout}s: {Exception}", time.TotalSeconds, ex.Message);
				});

			policy.Execute(InitiateConnection);
		}


		return true;
	}

	private void InitiateConnection()
	{
		_smtpClient = new SmtpClient();
		_smtpClient.Connect(_emailProviderSetting.Host, _emailProviderSetting.Port, SecureSocketOptions.StartTls);
		_smtpClient.Authenticate(_emailProviderSetting.Username, _emailProviderSetting.Password);
	}
}
