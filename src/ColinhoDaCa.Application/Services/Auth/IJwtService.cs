using ColinhoDaCa.Application.UseCases.Auth.v1.Login;

namespace ColinhoDaCa.Application.Services.Auth;

public interface IJwtService
{
    string GenerateToken(UsuarioResponse usuario);
}
