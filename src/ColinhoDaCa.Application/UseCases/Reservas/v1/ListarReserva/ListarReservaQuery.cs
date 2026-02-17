using ColinhoDaCa.Application._Shared.DTOs.Paginacao;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.ListarReserva;

public class ListarReservaQuery
{
    public long? ClienteId { get; set; }
    public string? ClienteNome { get; set; }
    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }
    public PaginacaoDto Paginacao { get; set; }
}