using MotoLocadora.Domain.Entities;

namespace MotoLocadora.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task InvalidateAsync(RefreshToken token);
}
