namespace MotoLocadora.Domain.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
}