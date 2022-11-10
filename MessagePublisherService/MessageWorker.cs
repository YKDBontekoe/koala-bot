using Discord;
using Discord.WebSocket;
using Infrastructure.Common.Constants;
using Infrastructure.Common.Models;
using Infrastructure.Messaging.Handlers.Interfaces;
using Koala.DiscordMessageService.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Koala.DiscordMessageService;

public class MessageWorker : IHostedService, IMessageHandlerCallback
{
    private readonly IMessageHandler _messageHandler;
    private readonly IMessageService _messageService;
    private readonly ILoggingService _loggingService;

    public MessageWorker(IMessageHandler messageHandler, IMessageService messageService, ILoggingService loggingService)
    {
        _messageHandler = messageHandler;
        _messageService = messageService;
        _loggingService = loggingService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _messageHandler.Start(this);
        _messageService.Initialize();
        _loggingService.Initialize();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _messageHandler.Stop();
        return Task.CompletedTask;
    }

    public Task<bool> HandleMessageAsync(string messageType, string message)
    {
        return Task.FromResult(true);
    }
}