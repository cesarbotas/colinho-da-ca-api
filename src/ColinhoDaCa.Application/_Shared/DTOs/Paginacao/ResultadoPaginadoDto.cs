using System.Diagnostics.CodeAnalysis;

namespace ColinhoDaCa.Application._Shared.DTOs.Paginacao;

[ExcludeFromCodeCoverage]
/// <summary>
/// Classe base de retornos paginados.
/// </summary>
/// <typeparam name="T">Tipo do retorno.</typeparam>
public class ResultadoPaginadoDto<T>
{
    public ResultadoPaginadoDto()
    {
    }

    public ResultadoPaginadoDto(IEnumerable<T> data)
    {
        Data = data;
    }

    public ResultadoPaginadoDto(IEnumerable<T> data, int total, PaginacaoDto pagination)
    {
        Data = data;
        Page = pagination.NumeroPagina;
        PageSize = pagination.QuantidadeRegistros;
        Total = total;
    }

    public ResultadoPaginadoDto(IEnumerable<T> data, int numeroPagina, int quantidadeRegistros, int totalRegistros)
    {
        Data = data;
        Page = numeroPagina;
        PageSize = quantidadeRegistros;
        Total = totalRegistros;
    }

    /// <summary>
    /// Obtém ou define o numero da pagina.
    /// </summary>
    public int Page { get; set; } = 0;

    /// <summary>
    /// Obtém ou define a quantidade de registros.
    /// </summary>
    public int PageSize { get; set; } = 0;

    /// <summary>
    /// Obtém ou define o total de registros resultantes da consulta.
    /// </summary>
    public int Total { get; set; }
    public IEnumerable<T>? Data { get; set; }
}