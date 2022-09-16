using System.Net.Sockets;
using Mailing.Worker.Abstracts;
using Mailing.Worker.SettingOptions;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Polly;
using SharedCommon.Modules.LoggerAdapter;

namespace Mailing.Worker.Services;

internal class MailProviderConnection : IMailProviderConnection
{
	private readonly MailProviderAppSetting _emailProviderSetting;
	private readonly object _locker = new();
	private readonly ILoggerAdapter<MailProviderConnection> _logger;

	private ISmtpClient? _smtpClient;

	public MailProviderConnection(IOptions<MailProviderAppSetting> mailProviderSettingOptions,
		ILoggerAdapter<MailProviderConnection> logger)
	{
		_emailProviderSetting = mailProviderSettingOptions.Value;
		_logger = logger;

		InitiateConnection();
	}

	public ISmtpClient SmtpClient()
	{
		if (!IsConnected)
		{
			_logger.LogWarning("SmtpClient is null, trying to reconnect");

			if (TryConnect())
			{
				return _smtpClient!;
			}

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
				.WaitAndRetry(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
					(ex, time) =>
					{
						_logger.LogWarning("Error while connecting to smtp server after {Timeout}s: {Exception}",
							time.TotalSeconds, ex.Message);
					});

			policy.Execute(InitiateConnection);
		}


		return true;
	}

	private void InitiateConnection()
	{
		_smtpClient = new SmtpClient();
		var secureSocketOpts = _emailProviderSetting.StartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.None;
		_smtpClient.Connect(_emailProviderSetting.Host, _emailProviderSetting.Port, secureSocketOpts);

		if (secureSocketOpts == SecureSocketOptions.None && _emailProviderSetting.LocalSmtp)
		{
			return; // skip authenticate if in local development mode
		}

		_smtpClient.Authenticate(_emailProviderSetting.Username, _emailProviderSetting.Password);
	}
}
