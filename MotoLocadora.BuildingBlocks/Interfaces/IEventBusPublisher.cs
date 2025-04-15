namespace MotoLocadora.BuildingBlocks.Interfaces;

public interface IEventBusPublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message);
}
