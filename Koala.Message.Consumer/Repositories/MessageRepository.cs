using Koala.MessageConsumerService.Models;
using Koala.MessageConsumerService.Options;
using Koala.MessageConsumerService.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Koala.MessageConsumerService.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly IMongoDatabase _database;

    public MessageRepository(IOptions<MongoConfiguration> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.Database);
    }

    public async Task AddMessageAsync(BaseMessage message)
    {
        await _database.GetCollection<BaseMessage>("Messages").InsertOneAsync(message);
    }
}