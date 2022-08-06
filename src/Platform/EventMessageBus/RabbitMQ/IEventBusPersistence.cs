namespace EventBusMessage.RabbitMQ;
public interface IEventBusPersistence
{
    /// <summary>
    /// Check whether the channel is connected to the RabbitMQ.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Attempt to re-connect the RabbitMQ.
    /// </summary>
    /// <returns></returns>
    bool TryConnect();

    /// <summary>
    /// Get the connection of channel.
    /// </summary>
    IConnection? Connection { get; }

    /// <summary>
    /// Create a new channel for connection.
    /// </summary>
    /// <returns></returns>
    IModel CreateChannel();
}
