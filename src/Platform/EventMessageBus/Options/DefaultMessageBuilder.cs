using EventBusMessage.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace EventBusMessage.Options;
public class DefaultMessageBuilder : IMessageBuilder
{
    public IServiceCollection ServiceCollection { get; }

    public DefaultMessageBuilder(IServiceCollection services)
    {
        ServiceCollection = services;
    }
}
