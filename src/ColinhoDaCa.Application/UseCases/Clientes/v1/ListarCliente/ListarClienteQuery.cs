using ColinhoDaCa.Application._Shared.DTOs.Paginacao;

namespace ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;

public class ListarClienteQuery
{
    public long? Id { get; set; }
    public PaginacaoDto Paginacao { get; set; } = new PaginacaoDto();
}