using ColinhoDaCa.Application._Shared.DTOs.Paginacao;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.ListarReserva;

public class ListarReservaQuery
{
    public long? ClienteId { get; set; }
    public PaginacaoDto Paginacao { get; set; }
}