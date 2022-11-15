namespace Infrastructure.Messaging.Handlers.Interfaces;

public interface IMessageHandlerCallback
{
    Task<bool> HandleMessageAsync(string messageType, string message);
}