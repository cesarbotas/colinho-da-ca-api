using ColinhoDaCa.Domain.RefreshTokens.Entities;
using ColinhoDaCa.Domain.RefreshTokens.Repositories;
using ColinhoDaCa.Infra.Data._Shared.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.Infra.Data.Context.Repositories.RefreshTokens;

public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    private readonly ColinhoDaCaContext _context;

    public RefreshTokenRepository(ColinhoDaCaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow);
    }

    public async Task RevokeAllUserTokensAsync(long usuarioId)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UsuarioId == usuarioId && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
        }
    }
}