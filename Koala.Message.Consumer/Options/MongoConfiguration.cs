namespace Koala.MessageConsumerService.Options;

public class MongoConfiguration
{
    public string ConnectionString { get; set; }
    public string Database { get; set; }  
    
    public string CollectionName { get; set; }
}