using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.RefreshTokens.Entities;

namespace ColinhoDaCa.Domain.RefreshTokens.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task RevokeAllUserTokensAsync(long usuarioId);
}