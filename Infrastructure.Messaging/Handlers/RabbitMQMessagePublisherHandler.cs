using System.Text;
using Infrastructure.Common.Constants;
using Infrastructure.Messaging.Handlers.Interfaces;
using Polly;
using RabbitMQ.Client;
using Serilog;

namespace Infrastructure.Messaging.Handlers;

public class RabbitMqMessagePublisherHandler : IMessagePublisher, IDisposable
{
    private const int DefaultPort = 5672;
    private readonly string _exchange;
    private readonly List<string> _hosts;
    private readonly string _password;
    private readonly int _port;
    private readonly string _username;
    private IConnection _connection;
    private IModel _model;

    public RabbitMqMessagePublisherHandler(string host, string username, string password, string exchange)
        : this(new List<string> { host }, username, password, exchange, DefaultPort)
    {
    }

    public RabbitMqMessagePublisherHandler(string host, string username, string password, string exchange, int port)
        : this(new List<string> { host }, username, password, exchange, port)
    {
    }

    public RabbitMqMessagePublisherHandler(IEnumerable<string> hosts, string username, string password, string exchange)
        : this(hosts, username, password, exchange, DefaultPort)
    {
    }

    public RabbitMqMessagePublisherHandler(IEnumerable<string> hosts, string username, string password, string exchange,
        int port)
    {
        _hosts = new List<string>(hosts);
        _port = port;
        _username = username;
        _password = password;
        _exchange = exchange;

        var logMessage = new StringBuilder();
        logMessage.AppendLine("Create RabbitMQ message-publisher instance using config:");
        logMessage.AppendLine($" - Hosts: {string.Join(',', _hosts.ToArray())}");
        logMessage.AppendLine($" - Port: {_port}");
        logMessage.AppendLine($" - UserName: {_username}");
        logMessage.AppendLine($" - Password: {new string('*', _password.Length)}");
        logMessage.Append($" - Exchange: {_exchange}");
        Log.Information(logMessage.ToString());

        Connect();
    }

    private RabbitMqMessagePublisherHandler()
    {
        Dispose();
    }

    public void Dispose()
    {
        _model?.Dispose();
        _model = null;
        _connection?.Dispose();
        _connection = null;
    }

    /// <summary>
    ///     Publish a message.
    /// </summary>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="routingKey">The routingkey to use (RabbitMQ specific).</param>
    public Task PublishMessageAsync(MessageTypes messageType, object message, RoutingKeys routingKey)
    {
        return Task.Run(() =>
        {
            var data = JsonMessageSerializerHandler.Serialize(message);
            var body = Encoding.UTF8.GetBytes(data);
            var properties = _model.CreateBasicProperties();
            properties.Headers = new Dictionary<string, object> { { "MessageType", messageType } };
            _model.BasicPublish(_exchange, routingKey.Equals(RoutingKeys.NONE) ? string.Empty : routingKey.ToString(), properties, body);
        });
    }

    private void Connect()
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(9, r => TimeSpan.FromSeconds(5),
                (ex, ts) => { Log.Error("Error connecting to RabbitMQ. Retrying in 5 sec."); })
            .Execute(() =>
            {
                var factory = new ConnectionFactory
                {
                    UserName = _username, Password = _password, Port = _port,
                    AutomaticRecoveryEnabled = true
                };
                _connection = factory.CreateConnection(_hosts);
                _model = _connection.CreateModel();
                _model.ExchangeDeclare(_exchange, "fanout", true, false);
            });
    }
}