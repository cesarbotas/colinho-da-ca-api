using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Clientes;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Infra.Data._Shared.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.Infra.Data.Context.Repositories.Clientes;

public class ClienteRepository : Repository<ClienteDb>, IClienteRepository, IClienteReadRepository
{
    private readonly ColinhoDaCaContext _context;

    public ClienteRepository(ColinhoDaCaContext context) 
        : base(context)
    {
        _context = context;
    }

    public IQueryable<ClienteDb> AsQueryable()
    {
        return _context.Clientes
            .AsQueryable();
    }

    public async Task<ClienteDb> GetByCpfAsync(string cpf)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Cpf == cpf);
    }

    public async Task<ResultadoPaginadoDto<ClientesDto>> PesquisarClientesDto(ListarClienteQuery query)
    {
        try
        {
            var queryResult =
                from u in _context.Clientes
                select new ClientesDto
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Celular = u.Celular,
                    Cpf = u.Cpf,
                    Observacoes = u.Observacoes
                };

            if (query.Id.HasValue && query.Id > 0)
            {
                queryResult = queryResult.Where(c => c.Id == query.Id);
            }

            var totalItens = await queryResult
                .CountAsync();

            var result = await queryResult
                .Skip((query.Paginacao.NumeroPagina - 1) * query.Paginacao.QuantidadeRegistros)
                .Take(query.Paginacao.QuantidadeRegistros)
                .ToListAsync();

            return new ResultadoPaginadoDto<ClientesDto>
            {
                Page = query.Paginacao.NumeroPagina,
                PageSize = query.Paginacao.QuantidadeRegistros,
                Total = totalItens,
                Data = result
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
}