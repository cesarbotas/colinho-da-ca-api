using ColinhoDaCa.Application.UseCases.Auth.v1.Login;

namespace ColinhoDaCa.Application.UseCases.Auth.v1.RefreshToken;

public interface IRefreshTokenService
{
    Task<LoginResponse> Handle(RefreshTokenCommand command);
}