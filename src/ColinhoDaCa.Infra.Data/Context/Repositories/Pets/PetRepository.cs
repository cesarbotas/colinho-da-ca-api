using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Pets;
using ColinhoDaCa.Application.UseCases.Pets.v1.ListarPet;
using ColinhoDaCa.Domain.Pets.Entities;
using ColinhoDaCa.Domain.Pets.Repositories;
using ColinhoDaCa.Infra.Data._Shared.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.Infra.Data.Context.Repositories.Pets;

public class PetRepository : Repository<Pet>, IPetRepository, IPetReadRepository
{
    private readonly ColinhoDaCaContext _context;

    public PetRepository(ColinhoDaCaContext context) 
        : base(context)
    {
        _context = context;
    }

    public IQueryable<Pet> AsQueryable()
    {
        return _context.Pets
            .AsQueryable();
    }

    public async Task<ResultadoPaginadoDto<PetsDto>> PesquisarPetsDto(ListarPetQuery query)
    {
        try
        {
            var queryResult =
                from p in _context.Pets
                join c in _context.Clientes on p.ClienteId equals c.Id
                join r in _context.Racas on p.RacaId equals r.Id into racaGroup
                from r in racaGroup.DefaultIfEmpty()
                select new PetsDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    RacaId = p.RacaId,
                    RacaNome = r.Nome,
                    Idade = p.Idade,
                    Peso = p.Peso,
                    Porte = p.Porte,
                    Observacoes = p.Observacoes,
                    ClienteId = p.ClienteId,
                    ClienteNome = c.Nome
                };

            if (query.ClienteId.HasValue && query.ClienteId > 0)
            {
                queryResult = queryResult
                    .Where(p => p.ClienteId == query.ClienteId);
            }

            var totalItens = await queryResult
                .CountAsync();

            var result = await queryResult
                .Skip((query.Paginacao.NumeroPagina - 1) * query.Paginacao.QuantidadeRegistros)
                .Take(query.Paginacao.QuantidadeRegistros)
                .ToListAsync();

            return new ResultadoPaginadoDto<PetsDto>
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