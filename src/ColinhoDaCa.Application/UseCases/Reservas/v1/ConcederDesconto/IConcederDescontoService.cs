namespace ColinhoDaCa.Application.UseCases.Reservas.v1.ConcederDesconto;

public interface IConcederDescontoService
{
    Task Handle(long reservaId, ConcederDescontoCommand command);
}
