using Infrastructure.Common.Constants;
using Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Koala.MessageHandlerService;

public class MessageWorker : IHostedService, IMessageHandlerCallback 
{
    private readonly IMessageHandler _messageHandler;

    public MessageWorker(IMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _messageHandler.Start(this);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _messageHandler.Stop();
        return Task.CompletedTask;
    }

    public Task<bool> HandleMessageAsync(string messageType, string message)
    {
        // Handle the message by message type
        switch (messageType)
        {
            case MessageTypes.MessageReceived:
                Console.WriteLine(message);
                break;
            default:
                // Handle the message
                Console.WriteLine($"Message type {messageType} not handled and has been ignored.");
                break;
        }
        
        return Task.FromResult(true);
    }
}