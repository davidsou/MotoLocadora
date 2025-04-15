namespace MotoLocadora.BuildingBlocks.Interfaces;

public interface IEventHandler<T>
{
    Task HandleAsync(T @event);
}
