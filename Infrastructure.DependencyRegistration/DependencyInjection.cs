using Infrastructure.DependencyRegistration.Interfaces;
using Infrastructure.Messaging.Handlers;
using Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyRegistration;

public class DependencyInjection : IDependencyInjection
{
    public IServiceCollection RegisterInfrastructureDependencies(IServiceCollection services)
    {
        services.AddScoped<IMessage, RabbitMqMessageHandler>();
    }
}