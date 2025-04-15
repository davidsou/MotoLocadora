namespace MotoLocadora.BuildingBlocks.Interfaces;


public interface IEventBusConsumer
{
    void StartConsuming(CancellationToken cancellationToken = default);
}
