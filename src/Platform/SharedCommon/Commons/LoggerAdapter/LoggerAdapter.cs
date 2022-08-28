using Microsoft.Extensions.Logging;

namespace SharedCommon.Commons.LoggerAdapter;

public interface ILoggerAdapter
{
	void LogInformation(string message, params object?[] args);
	void LogInformation(Exception ex, string message, params object?[] args);
	void LogWarning(string message, params object?[] args);
	void LogWarning(Exception ex, string message, params object?[] args);
	void LogError(string message, params object?[] args);
	void LogError(Exception ex, string message, params object?[] args);
	void LogCritical(string message, params object?[] args);
	void LogCritical(Exception ex, string message, params object?[] args);
}

public interface ILoggerAdapter<T> : ILoggerAdapter
{
}

public class LoggerAdapter<T> : ILoggerAdapter<T>
{
	private readonly ILogger<T> _logger;

	public LoggerAdapter(ILogger<T> logger)
	{
		_logger = logger;
	}

	public void LogInformation(string message, params object?[] args)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation(message, args);
		}
	}

	public void LogInformation(Exception ex, string message, params object?[] args)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation(ex, message, args);
		}
	}

	public void LogWarning(string message, params object?[] args)
	{
		if (_logger.IsEnabled(LogLevel.Warning))
		{
			_logger.LogWarning(message, args);
		}
	}

	public void LogWarning(Exception ex, string message, params object?[] args)
	{
		if (_logger.IsEnabled(LogLevel.Warning))
		{
			_logger.LogWarning(ex, message, args);
		}
	}

	public void LogError(string message, params object?[] args)
	{
		if (_logger.IsEnabled(LogLevel.Error))
		{
			_logger.LogError(message, args);
		}
	}

	public void LogError(Exception ex, string message, params object?[] args)
	{
		if (_logger.IsEnabled(LogLevel.Error))
		{
			_logger.LogError(ex, message, args);
		}
	}

	public void LogCritical(string message, params object?[] args)
	{
		if (_logger.IsEnabled(LogLevel.Critical))
		{
			_logger.LogCritical(message, args);
		}
	}

	public void LogCritical(Exception ex, string message, params object?[] args)
	{
		if (_logger.IsEnabled(LogLevel.Critical))
		{
			_logger.LogCritical(ex, message, args);
		}
	}
}
