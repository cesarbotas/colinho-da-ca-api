using System.Security.Claims;
using ColinhoDaCa.Application.UseCases.Auth.v1.Login;

namespace ColinhoDaCa.Application.Services.Auth;

public interface IJwtService
{
    string GenerateAccessToken(UsuarioResponse usuario);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
