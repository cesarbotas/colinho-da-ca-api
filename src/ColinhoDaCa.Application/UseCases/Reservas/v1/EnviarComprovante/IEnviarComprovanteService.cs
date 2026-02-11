namespace ColinhoDaCa.Application.UseCases.Reservas.v1.EnviarComprovante;

public interface IEnviarComprovanteService
{
    Task Handle(long reservaId, EnviarComprovanteCommand command);
}
