using Infrastructure.Messaging.Configuration;
using Koala.MessageConsumerService.Options;
using Koala.MessageConsumerService.Repositories;
using Koala.MessageConsumerService.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Koala.MessageConsumerService;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var host = Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.UseRabbitMQMessageHandler(hostContext.Configuration);
                services.Configure<MongoConfiguration>(hostContext.Configuration.GetSection("MongoDB"));
                services.AddScoped<IMessageRepository, MessageRepository>();
                
                services.AddHostedService<MessageConsumerWorker>();
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