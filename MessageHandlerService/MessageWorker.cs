using Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Koala.MessageHandlerService;

public class MessageWorker : IHostedService, IMessageHandlerCallback
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HandleMessageAsync(string messageType, string message)
    {
        throw new NotImplementedException();
    }
}