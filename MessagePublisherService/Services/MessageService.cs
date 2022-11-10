using Discord.WebSocket;
using Infrastructure.Common.Constants;
using Infrastructure.Common.Models;
using Infrastructure.Messaging.Handlers.Interfaces;
using Koala.DiscordMessageService.Services.Interfaces;

namespace Koala.DiscordMessageService.Services;

public class MessageService : IMessageService
{
    private readonly BaseSocketClient _client;
    private readonly IMessagePublisher _publisher;
    
    public MessageService(BaseSocketClient client, IMessagePublisher publisher)
    {
        _client = client;
        _publisher = publisher;
    }

    // Read all incoming messages and log them
    private async Task Client_MessageReceived(SocketMessage arg)
    {
        if (arg.Author.IsBot) return;
        if (arg is not SocketUserMessage message) return;
        
        var messageReceived = new UserMessageReceived()
        {
            Message = message.Content,
            Channel = new BaseChannel
            {
                Id = message.Channel.Id,
                Name = message.Channel.Name
            },
            User = new BaseUser
            {
                Id = message.Author.Id,
                Username = message.Author.Username
            }
        };
        
        await _publisher.PublishMessageAsync(MessageTypes.MESSAGE_RECEIVED, messageReceived, RoutingKeys.NONE);
    }

    public void Initialize()
    {
        _client.MessageReceived += Client_MessageReceived;
    }
}