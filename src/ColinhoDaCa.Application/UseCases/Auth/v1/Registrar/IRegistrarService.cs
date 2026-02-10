namespace ColinhoDaCa.Application.UseCases.Auth.v1.Registrar;

public interface IRegistrarService
{
    Task Handle(RegistrarCommand command);
}
