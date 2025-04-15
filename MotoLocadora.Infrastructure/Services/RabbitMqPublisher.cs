using Microsoft.Extensions.Options;
using MotoLocadora.BuildingBlocks.Interfaces;
using MotoLocadora.BuildingBlocks.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Polly;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace MotoLocadora.Infrastructure.Services;

public class RabbitMqPublisher : IEventBusPublisher, IDisposable
{
    private readonly EventBusOptions _config;
    private readonly IConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMqPublisher(IOptions<EventBusOptions> options)
    {
        _config = options.Value;

        _factory = new ConnectionFactory
        {
            Uri = new Uri(_config.HostAddress),
            UserName = _config.UserName,
            Password = _config.Password,
            Ssl = { Enabled = _config.Ssl }
        };
    }

    private void EnsureConnection()
    {
        if (_connection is { IsOpen: true } && _channel is { IsOpen: true })
            return;

        var policy = Policy
            .Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        policy.Execute(() =>
        {
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            SetupInfrastructure();
        });
    }

    private void SetupInfrastructure()
    {
        foreach (var exchange in _config.Exchanges)
        {
            _channel.ExchangeDeclare(exchange.Name, exchange.Type, durable: true, autoDelete: false, arguments: null);
        }

        foreach (var queue in _config.Queues)
        {
            _channel.QueueDeclare(queue.Name, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue.Name, queue.Exchange, queue.RoutingKey);
        }
    }

    public Task PublishAsync<T>(string exchange, string routingKey, T message)
    {
        EnsureConnection();

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(
            exchange: exchange,
            routingKey: routingKey,
            basicProperties: null,
            body: body
        );

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
