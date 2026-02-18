namespace ColinhoDaCa.Application.UseCases.Reservas.v1.AprovarPagamento;

public interface IAprovarPagamentoService
{
    Task Handle(long reservaId);
}
