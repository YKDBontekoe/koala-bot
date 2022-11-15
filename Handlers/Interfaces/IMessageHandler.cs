namespace Infrastructure.Messaging.Handlers.Interfaces;

public interface IMessageHandler
{
    void Start(IMessageHandlerCallback callback);
    void Stop();
}