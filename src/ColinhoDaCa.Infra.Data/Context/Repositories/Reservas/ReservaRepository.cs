using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Reservas;
using ColinhoDaCa.Application.UseCases.Reservas.v1.ListarReserva;
using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Reservas.Repositories;
using ColinhoDaCa.Infra.Data._Shared.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Infra.Data.Context.Repositories.Reservas;

public class ReservaRepository : Repository<Reserva>, IReservaRepository, IReservaReadRepository
{
    private readonly ColinhoDaCaContext _context;
    private readonly ILogger<ReservaRepository> _logger;

    public ReservaRepository(ColinhoDaCaContext context,
    ILogger<ReservaRepository> logger) 
        : base(context)
    {
        _context = context;
        _logger = logger;
    }

    public IQueryable<Reserva> AsQueryable()
    {
        return _context.Reservas
            .AsQueryable();
    }

    public async Task<Reserva?> GetWithRelationsAsync(long id)
    {
        return await _context.Reservas
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<ResultadoPaginadoDto<ReservasDto>> PesquisarReservasDto(ListarReservaQuery query)
    {
        try
        {
            // Query principal otimizada
            var reservasQuery = _context.Reservas
                .Where(r => query.ClienteId == null || r.ClienteId == query.ClienteId)
                .OrderByDescending(r => r.Id);

            var totalItens = await reservasQuery.CountAsync();

            var reservas = await reservasQuery
                .Skip((query.Paginacao.NumeroPagina - 1) * query.Paginacao.QuantidadeRegistros)
                .Take(query.Paginacao.QuantidadeRegistros)
                .ToListAsync();

            // Buscar dados relacionados separadamente
            var clienteIds = reservas.Select(r => r.ClienteId).Distinct().ToList();
            var clientes = await _context.Clientes
                .Where(c => clienteIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, c => c.Nome);

            var reservaIds = reservas.Select(r => r.Id).ToList();
            
            var reservaPets = await _context.ReservaPets
                .Where(rp => reservaIds.Contains(rp.ReservaId))
                .ToListAsync();
            
            var petIds = reservaPets.Select(rp => rp.PetId).Distinct().ToList();
            var pets = await _context.Pets
                .Where(p => petIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => new { p.Nome, p.RacaId });
            
            var racaIds = pets.Values.Where(p => p.RacaId.HasValue).Select(p => p.RacaId.Value).Distinct().ToList();
            var racas = await _context.Racas
                .Where(r => racaIds.Contains(r.Id))
                .ToDictionaryAsync(r => r.Id, r => r.Nome);
            
            var statusHistorico = await _context.ReservaStatusHistorico
                .Where(h => reservaIds.Contains(h.ReservaId))
                .OrderBy(h => h.DataAlteracao)
                .ToListAsync();
            
            var usuarioIds = statusHistorico.Select(h => h.UsuarioId).Distinct().ToList();
            var usuarios = await _context.Usuarios
                .Where(u => usuarioIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.ClienteId);
            
            var usuarioClienteIds = usuarios.Values.Distinct().ToList();
            var usuarioClientes = await _context.Clientes
                .Where(c => usuarioClienteIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, c => c.Nome);

            var result = reservas.Select(r => new ReservasDto
            {
                Id = r.Id,
                ClienteId = r.ClienteId,
                ClienteNome = clientes.GetValueOrDefault(r.ClienteId, ""),
                DataInicial = r.DataInicial,
                DataFinal = r.DataFinal,
                QuantidadeDiarias = r.QuantidadeDiarias,
                QuantidadePets = r.QuantidadePets,
                ValorTotal = r.ValorTotal,
                ValorDesconto = r.ValorDesconto,
                ValorFinal = r.ValorFinal,
                Observacoes = r.Observacoes,
                Status = (int)r.Status,
                ComprovantePagamento = r.ComprovantePagamento,
                DataPagamento = r.DataPagamento,
                ObservacoesPagamento = r.ObservacoesPagamento,
                StatusTimeline = new Dictionary<int, bool>
                {
                    { 1, (int)r.Status >= 1 },
                    { 2, (int)r.Status >= 2 },
                    { 3, (int)r.Status >= 3 },
                    { 4, (int)r.Status >= 4 },
                    { 5, (int)r.Status >= 5 },
                    { 6, (int)r.Status == 6 }
                },
                Pets = reservaPets
                    .Where(rp => rp.ReservaId == r.Id)
                    .Select(rp => new PetReservaDto
                    {
                        Id = rp.PetId,
                        Nome = pets.GetValueOrDefault(rp.PetId)?.Nome ?? "",
                        RacaNome = pets.GetValueOrDefault(rp.PetId)?.RacaId.HasValue == true 
                            ? racas.GetValueOrDefault(pets[rp.PetId].RacaId.Value, "")
                            : null
                    }).ToList(),
                Historico = statusHistorico
                    .Where(h => h.ReservaId == r.Id)
                    .Select(h => new StatusHistoricoDto
                    {
                        Status = (int)h.Status,
                        UsuarioId = h.UsuarioId,
                        UsuarioNome = usuarios.ContainsKey(h.UsuarioId) 
                            ? usuarioClientes.GetValueOrDefault(usuarios[h.UsuarioId], "")
                            : "",
                        DataAlteracao = h.DataAlteracao
                    }).ToList()
            }).ToList();

            return new ResultadoPaginadoDto<ReservasDto>
            {
                Page = query.Paginacao.NumeroPagina,
                PageSize = query.Paginacao.QuantidadeRegistros,
                Total = totalItens,
                Data = result
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PesquisarReservasDto] - Erro ao pesquisar reservas: {Message}", ex.Message);

            throw;
        }
    }
}
