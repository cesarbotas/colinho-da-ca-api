using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Cupons;
using ColinhoDaCa.Domain.Cupons.Repositories;

namespace ColinhoDaCa.Application.UseCases.Cupons.v1.ListarCupom;

public class ListarCupomService : IListarCupomService
{
    private readonly ICupomRepository _cupomRepository;

    public ListarCupomService(ICupomRepository cupomRepository)
    {
        _cupomRepository = cupomRepository;
    }

    public async Task<ResultadoPaginadoDto<CupomDto>> Handle(ListarCupomQuery query)
    {
        var cupons = await _cupomRepository.GetAllAsync();

        var total = cupons.Count;
        var paginados = cupons
            .Skip((query.Paginacao.NumeroPagina - 1) * query.Paginacao.QuantidadeRegistros)
            .Take(query.Paginacao.QuantidadeRegistros)
            .Select(c => new CupomDto
            {
                Id = c.Id,
                Codigo = c.Codigo,
                Descricao = c.Descricao,
                Tipo = (int)c.Tipo,
                Percentual = c.Percentual,
                ValorFixo = c.ValorFixo,
                MinimoValorTotal = c.MinimoValorTotal,
                MinimoPets = c.MinimoPets,
                MinimoDiarias = c.MinimoDiarias,
                DataInicio = c.DataInicio,
                DataFim = c.DataFim,
                Ativo = c.Ativo
            })
            .ToList();

        return new ResultadoPaginadoDto<CupomDto>
        {
            Page = query.Paginacao.NumeroPagina,
            PageSize = query.Paginacao.QuantidadeRegistros,
            Total = total,
            Data = paginados
        };
    }
}