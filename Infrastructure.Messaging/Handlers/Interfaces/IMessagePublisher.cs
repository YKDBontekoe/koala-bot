using Infrastructure.Common.Constants;

namespace Infrastructure.Messaging.Handlers.Interfaces;

public interface IMessagePublisher
{
    Task PublishMessageAsync(MessageTypes messageType, object message, RoutingKeys routingKey);
}