namespace ColinhoDaCa.Application.UseCases.Sobre.v1.EnviarEmail;

public interface IEnviarEmailService
{
    Task Handle(EnviarEmailCommand command);
}