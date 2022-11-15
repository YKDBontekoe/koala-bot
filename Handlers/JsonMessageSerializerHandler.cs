using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.Messaging.Handlers;

public static class JsonMessageSerializerHandler
{
    private static readonly JsonSerializerSettings SerializerSettings;

    static JsonMessageSerializerHandler()
    {
        SerializerSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };
        SerializerSettings.Converters.Add(new StringEnumConverter
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        });
    }

    public static string Serialize(object message)
    {
        return JsonConvert.SerializeObject(message, SerializerSettings);
    }

    public static JObject Deserialize(string message)
    {
        return JsonConvert.DeserializeObject<JObject>(message, SerializerSettings) ??
               throw new InvalidOperationException();
    }
}