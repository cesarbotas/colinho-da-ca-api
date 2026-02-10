using ColinhoDaCa.Application._Shared.DTOs.Paginacao;

namespace ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;

public class ListarClienteQuery
{
    public PaginacaoDto Paginacao { get; set; } = new PaginacaoDto();
}