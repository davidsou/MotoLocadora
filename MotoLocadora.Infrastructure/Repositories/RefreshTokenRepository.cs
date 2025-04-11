using Microsoft.EntityFrameworkCore;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;
using MotoLocadora.Infrastructure.Context;

namespace MotoLocadora.Infrastructure.Repositories;


public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppSqlContext _context;

    public RefreshTokenRepository(AppSqlContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken token)
    {
        await _context.Set<RefreshToken>().AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.Set<RefreshToken>()
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked);
    }

    public async Task InvalidateAsync(RefreshToken token)
    {
        token.IsRevoked = true;
        _context.Set<RefreshToken>().Update(token);
        await _context.SaveChangesAsync();
    }
}
