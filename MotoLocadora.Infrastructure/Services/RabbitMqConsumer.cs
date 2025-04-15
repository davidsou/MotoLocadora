using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MotoLocadora.BuildingBlocks.Interfaces;
using MotoLocadora.BuildingBlocks.Options;
using MotoLocadora.Domain.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MotoLocadora.Infrastructure.Services;

public class RabbitMqConsumer : BackgroundService, IEventBusConsumer
{
    private readonly EventBusOptions _config;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider; // Para resolver os handlers dinamicamente

    public RabbitMqConsumer(
        IOptions<EventBusOptions> options,
        IServiceProvider serviceProvider)
    {
        _config = options.Value;
        _serviceProvider = serviceProvider;

        var factory = new ConnectionFactory
        {
            Uri = new Uri(_config.HostAddress),
            UserName = _config.UserName,
            Password = _config.Password,
            Ssl = { Enabled = _config.Ssl }
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        SetupInfrastructure();
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

    public void StartConsuming(CancellationToken cancellationToken = default)
    {
        foreach (var queue in _config.Queues)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    await DispatchEvent(queue.RoutingKey, message);
                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro processando mensagem: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };

            _channel.BasicConsume(queue: queue.Name, autoAck: false, consumer: consumer);
        }
    }

    private async Task DispatchEvent(string routingKey, string message)
    {
        // Cria escopo para resolver dependências dos handlers
        using var scope = _serviceProvider.CreateScope();

        // Aqui, você mapeia o routing key para o tipo de evento esperado
        switch (routingKey)
        {
            case "rent.created":
                var rentCreatedEvent = JsonSerializer.Deserialize<RentCreatedEvent>(message);
                if (rentCreatedEvent is not null)
                {
                    var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<RentCreatedEvent>>();
                    await handler.HandleAsync(rentCreatedEvent);
                }
                break;
            case "motorcycle.created":
                var motorcycleCreatedEvent = JsonSerializer.Deserialize<MotorcycleCreatedEvent>(message);
                if (motorcycleCreatedEvent is not null)
                {
                    var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<MotorcycleCreatedEvent>>();
                    await handler.HandleAsync(motorcycleCreatedEvent);
                }
                break;
            default:
                Console.WriteLine($"[Aviso] Nenhum handler registrado para routing key: {routingKey}");
                break;
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        StartConsuming(stoppingToken);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}
