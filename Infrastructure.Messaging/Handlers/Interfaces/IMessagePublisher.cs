using Infrastructure.Common.Constants;

namespace Infrastructure.Messaging.Handlers.Interfaces;

public interface IMessagePublisher
{
    Task PublishMessageAsync(string messageType, object message, RoutingKeys routingKey);
}