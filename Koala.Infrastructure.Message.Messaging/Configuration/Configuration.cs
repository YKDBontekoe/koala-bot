using System.Text;
using Infrastructure.Messaging.Handlers;
using Infrastructure.Messaging.Handlers.Interfaces;
using Koala.Infrastructure.Messaging.Handlers;
using Koala.Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.Configuration;

public static class Configuration
{
    private const int DefaultPort = 5672;
    private static string _host = string.Empty;
    private static string _userName= string.Empty;
    private static string _password = string.Empty;
    private static string _exchange = string.Empty;
    private static string _queue = string.Empty;
    private static string _routingKey = string.Empty;
    private static int _port;
    private static List<string> _errors = new ();
    private static bool _isValid;
    
    public static void UseRabbitMQMessagePublisher(this IServiceCollection services, IConfiguration config)
    {
        GetRabbitMqSettings(config, "RabbitMQPublisher");
        services.AddTransient<IMessagePublisher>(_ => new RabbitMqMessagePublisherHandler(
            _host, _userName, _password, _exchange, _port));
    }
    public static void UseRabbitMQMessageHandler(this IServiceCollection services, IConfiguration config)
    {
        GetRabbitMqSettings(config, "RabbitMQHandler");
        services.AddTransient<IMessageHandler>(_ => new RabbitMqMessageHandler(
            _host, _userName, _password, _exchange, _queue, _routingKey, _port));
    }

    private static void GetRabbitMqSettings(IConfiguration config, string sectionName)
    {
        _isValid = true;
        _errors = new List<string>();

        var configSection = config.GetSection(sectionName);
        if (!configSection.Exists()) throw new Exception($"Required config-section '{sectionName}' not found.");

        // get configuration settings
        DetermineHost(configSection);
        DeterminePort(configSection);
        DetermineUsername(configSection);
        DeterminePassword(configSection);
        DetermineExchange(configSection);
        if (sectionName == "RabbitMQHandler")
        {
            DetermineQueue(configSection);
            DetermineRoutingKey(configSection);
        }

        // handle possible errors
        if (_isValid) return;
        var errorMessage = new StringBuilder("Invalid RabbitMQ configuration:");
        _errors.ForEach(e => errorMessage.AppendLine(e));
        throw new Exception(errorMessage.ToString());
    }

    private static void DetermineHost(IConfigurationSection configSection)
    {
        _host = configSection["Host"];
        if (string.IsNullOrEmpty(_host))
        {
            _errors.Add("Required config-setting 'Host' not found.");
            _isValid = false;
        }
    }

    private static void DeterminePort(IConfigurationSection configSection)
    {
        var portSetting = configSection["Port"];
        if (string.IsNullOrEmpty(portSetting))
        {
            _port = DefaultPort;
        }
        else
        {
            if (int.TryParse(portSetting, out var result))
            {
                _port = result;
            }
            else
            {
                _isValid = false;
                _errors.Add("Unable to parse config-setting 'Port' into an integer.");
            }
        }
    }

    private static void DetermineUsername(IConfigurationSection configSection)
    {
        _userName = configSection["UserName"];
        if (!string.IsNullOrEmpty(_userName)) return;
        _isValid = false;
        _errors.Add("Required config-setting 'UserName' not found.");
    }

    private static void DeterminePassword(IConfigurationSection configSection)
    {
        _password = configSection["Password"];
        if (!string.IsNullOrEmpty(_password)) return;
        _isValid = false;
        _errors.Add("Required config-setting 'Password' not found.");
    }

    private static void DetermineExchange(IConfigurationSection configSection)
    {
        _exchange = configSection["Exchange"];
        if (!string.IsNullOrEmpty(_exchange)) return;
        _isValid = false;
        _errors.Add("Required config-setting 'Exchange' not found.");
    }

    private static void DetermineQueue(IConfigurationSection configSection)
    {
        _queue = configSection["Queue"];
        if (!string.IsNullOrEmpty(_queue)) return;
        _isValid = false;
        _errors.Add("Required config-setting 'Queue' not found.");
    }

    private static void DetermineRoutingKey(IConfigurationSection configSection)
    {
        _routingKey = configSection["RoutingKey"] ?? "";
    }
}