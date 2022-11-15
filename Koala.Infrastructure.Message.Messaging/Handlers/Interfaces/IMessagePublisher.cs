namespace Koala.Infrastructure.Messaging.Handlers.Interfaces;

public interface IMessagePublisher
{
    Task PublishMessageAsync(string messageType, object message, string routingKey);
}