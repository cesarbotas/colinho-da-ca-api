namespace ColinhoDaCa.Application.UseCases.Auth.v1.Login;

public interface ILoginService
{
    Task<LoginResponse> Handle(LoginCommand command);
}
