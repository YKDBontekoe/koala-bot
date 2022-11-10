using Discord;
using Discord.WebSocket;
using Infrastructure.Messaging.Configuration;
using Infrastructure.Messaging.Handlers;
using Infrastructure.Messaging.Handlers.Interfaces;
using Koala.DiscordMessageService.Services;
using Koala.DiscordMessageService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using InvalidOperationException = System.InvalidOperationException;

namespace Koala.DiscordMessageService;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var host = Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.UseRabbitMQMessagePublisher(hostContext.Configuration);
                services.AddHostedService<MessageWorker>();
                
                var config = new DiscordSocketConfig
                {
                    AlwaysDownloadUsers = true,
                    GatewayIntents = GatewayIntents.All
                };
                var client = new DiscordSocketClient(config);

                client.LoginAsync(TokenType.Bot, hostContext.Configuration["Discord:Token"]);
                client.StartAsync();
                
                services.AddTransient<ILoggingService>(_ => 
                    new LoggingService(client));

                services.UseRabbitMQMessagePublisher(hostContext.Configuration);
                services.UseRabbitMQMessageHandler(hostContext.Configuration);
                services.AddTransient<IMessageService>(_ => new global::Koala.DiscordMessageService.Services.MessageService(client,
                    services.BuildServiceProvider().GetService<IMessagePublisher>() ??
                    throw new InvalidOperationException()));
            })
            .UseSerilog((hostContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
            })
            .UseConsoleLifetime()
            .Build();

        await host.RunAsync();
    }
}