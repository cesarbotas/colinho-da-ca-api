namespace ColinhoDaCa.Application.Services.Auth;

public interface IJwtService
{
    string GenerateToken(string email, long userId);
}
