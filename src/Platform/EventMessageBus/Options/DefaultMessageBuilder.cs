using EventMessageBus.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace EventMessageBus.Options;
public class DefaultMessageBuilder : IMessageBuilder
{
    public IServiceCollection ServiceCollection { get; }

    public DefaultMessageBuilder(IServiceCollection services)
    {
        ServiceCollection = services;
    }
}
