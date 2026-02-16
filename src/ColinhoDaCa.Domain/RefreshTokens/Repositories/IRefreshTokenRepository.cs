using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.RefreshTokens.Entities;

namespace ColinhoDaCa.Domain.RefreshTokens.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshTokenDb>
{
    Task<RefreshTokenDb?> GetByTokenAsync(string token);
    Task RevokeAllUserTokensAsync(long usuarioId);
}