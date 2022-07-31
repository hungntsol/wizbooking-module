namespace Mailing.Worker.Abstracts;
internal interface IMailProviderConnection
{
	/// <summary>
	/// Get the connection to the provider.
	/// </summary>
	ISmtpClient SmtpClient();

	/// <summary>
	/// Check if the connection is still open.
	/// </summary>
	bool IsConnected { get; }

	/// <summary>
	/// Try connection provider.
	/// </summary>
	/// <returns></returns>
	bool TryConnect();
}
