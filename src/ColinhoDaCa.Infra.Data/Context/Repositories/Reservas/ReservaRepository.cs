using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Reservas;
using ColinhoDaCa.Application.UseCases.Reservas.v1.ListarReserva;
using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Reservas.Repositories;
using ColinhoDaCa.Infra.Data._Shared.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.Infra.Data.Context.Repositories.Reservas;

public class ReservaRepository : Repository<ReservaDb>, IReservaRepository, IReservaReadRepository
{
    private readonly ColinhoDaCaContext _context;

    public ReservaRepository(ColinhoDaCaContext context) 
        : base(context)
    {
        _context = context;
    }

    public IQueryable<ReservaDb> AsQueryable()
    {
        return _context.Reservas
            .AsQueryable();
    }

    public async Task<ResultadoPaginadoDto<ReservasDto>> PesquisarReservasDto(ListarReservaQuery query)
    {
        try
        {
            var queryResult =
                from r in _context.Reservas
                join c in _context.Clientes on r.ClienteId equals c.Id
                where query.ClienteId == null || r.ClienteId == query.ClienteId
                select new
                {
                    r.Id,
                    r.ClienteId,
                    ClienteNome = c.Nome,
                    r.DataInicial,
                    r.DataFinal,
                    r.QuantidadeDiarias,
                    r.QuantidadePets,
                    r.ValorTotal,
                    r.ValorDesconto,
                    r.ValorFinal,
                    r.Observacoes,
                    r.Status,
                    r.ComprovantePagamento,
                    r.DataPagamento,
                    r.ObservacoesPagamento,
                    Pets = (from rp in _context.ReservaPets
                            join p in _context.Pets on rp.PetId equals p.Id
                            join ra in _context.Racas on p.RacaId equals ra.Id into racaGroup
                            from ra in racaGroup.DefaultIfEmpty()
                            where rp.ReservaId == r.Id
                            select new PetReservaDto
                            {
                                Id = p.Id,
                                Nome = p.Nome,
                                RacaNome = ra != null ? ra.Nome : null
                            }).ToList(),
                    Historico = (from h in _context.ReservaStatusHistorico
                                 join u in _context.Usuarios on h.UsuarioId equals u.Id
                                 join cl in _context.Clientes on u.ClienteId equals cl.Id
                                 where h.ReservaId == r.Id
                                 orderby h.DataAlteracao
                                 select new StatusHistoricoDto
                                 {
                                     Status = (int)h.Status,
                                     UsuarioId = h.UsuarioId,
                                     UsuarioNome = cl.Nome,
                                     DataAlteracao = h.DataAlteracao
                                 }).ToList()
                };

            var totalItens = await queryResult.CountAsync();

            var result = await queryResult
                .Skip((query.Paginacao.NumeroPagina - 1) * query.Paginacao.QuantidadeRegistros)
                .Take(query.Paginacao.QuantidadeRegistros)
                .Select(x => new ReservasDto
                {
                    Id = x.Id,
                    ClienteId = x.ClienteId,
                    ClienteNome = x.ClienteNome,
                    DataInicial = x.DataInicial,
                    DataFinal = x.DataFinal,
                    QuantidadeDiarias = x.QuantidadeDiarias,
                    QuantidadePets = x.QuantidadePets,
                    ValorTotal = x.ValorTotal,
                    ValorDesconto = x.ValorDesconto,
                    ValorFinal = x.ValorFinal,
                    Observacoes = x.Observacoes,
                    Status = (int)x.Status,
                    ComprovantePagamento = x.ComprovantePagamento,
                    DataPagamento = x.DataPagamento,
                    ObservacoesPagamento = x.ObservacoesPagamento,
                    StatusTimeline = new Dictionary<int, bool>
                    {
                        { 1, (int)x.Status >= 1 },
                        { 2, (int)x.Status >= 2 },
                        { 3, (int)x.Status >= 3 },
                        { 4, (int)x.Status >= 4 },
                        { 5, (int)x.Status >= 5 },
                        { 6, (int)x.Status == 6 }
                    },
                    Historico = x.Historico,
                    Pets = x.Pets
                })
                .ToListAsync();

            return new ResultadoPaginadoDto<ReservasDto>
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
