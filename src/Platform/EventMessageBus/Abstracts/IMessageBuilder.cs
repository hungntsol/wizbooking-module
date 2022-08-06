using Microsoft.Extensions.DependencyInjection;

namespace EventBusMessage.Abstracts;
public interface IMessageBuilder
{
    IServiceCollection ServiceCollection { get; }
}
