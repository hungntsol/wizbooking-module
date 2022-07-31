using Microsoft.Extensions.DependencyInjection;

namespace EventMessageBus.Abstracts;
public interface IMessageBuilder
{
    IServiceCollection ServiceCollection { get; }
}
