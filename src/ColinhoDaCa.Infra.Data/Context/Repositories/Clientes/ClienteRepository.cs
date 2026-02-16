using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Clientes;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Infra.Data._Shared.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.Infra.Data.Context.Repositories.Clientes;

public class ClienteRepository : Repository<Cliente>, IClienteRepository, IClienteReadRepository
{
    private readonly ColinhoDaCaContext _context;

    public ClienteRepository(ColinhoDaCaContext context) 
        : base(context)
    {
        _context = context;
    }

    public IQueryable<Cliente> AsQueryable()
    {
        return _context.Clientes
            .AsQueryable();
    }

    public async Task<Cliente> GetByCpfAsync(string cpf)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Cpf == cpf);
    }

    public async Task<Cliente> GetByEmailAsync(string email)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Email == email);
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

            if (query.ClienteId.HasValue && query.ClienteId > 0)
            {
                queryResult = queryResult.Where(c => c.Id == query.ClienteId);
            }

            if (!string.IsNullOrEmpty(query.Nome))
            {
                queryResult = queryResult.Where(c => c.Nome.ToLower().Contains(query.Nome.ToLower()));
            }

            if (!string.IsNullOrEmpty(query.Cpf))
            {
                queryResult = queryResult.Where(c => c.Cpf.ToLower().Contains(query.Cpf.ToLower()));
            }

            if (!string.IsNullOrEmpty(query.Email))
            {
                queryResult = queryResult.Where(c => c.Email.ToLower().Contains(query.Email.ToLower()));
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