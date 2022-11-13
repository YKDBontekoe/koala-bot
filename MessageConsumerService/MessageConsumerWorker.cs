using Infrastructure.Common.Constants;
using Infrastructure.Common.Models;
using Infrastructure.Messaging.Handlers;
using Infrastructure.Messaging.Handlers.Interfaces;
using Koala.MessageConsumerService.Repositories.Interfaces;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Koala.MessageConsumerService;

public class MessageConsumerWorker : IHostedService, IMessageHandlerCallback 
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMessageHandler _messageHandler;

    public MessageConsumerWorker(IMessageHandler messageHandler, IMessageRepository messageRepository)
    {
        _messageHandler = messageHandler;
        _messageRepository = messageRepository;
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
        try
        {
            switch (messageType)
            {
                case MessageTypes.MessageReceived:
                    var messageObject = JsonMessageSerializerHandler.Deserialize(message);
                    var baseMessageObject = messageObject.ToObject<BaseMessage>();

                    if (baseMessageObject is null)
                    {
                        Log.Error("Message type {MessageType} is not invalid", messageType);
                        break;
                    }
                    
                    _messageRepository.AddMessageAsync(baseMessageObject);
                    Log.Information("Message type {MessageType} has been consumed", messageType);
                    break;
                default:
                    // Handle the message
                    Console.WriteLine($"Message type {messageType} not handled and has been ignored.");
                    break;
            }
        } catch (Exception ex)
        {
            Log.Error("Error handling message: {ExMessage}", ex.Message);
        }

        return Task.FromResult(true);
    }
}