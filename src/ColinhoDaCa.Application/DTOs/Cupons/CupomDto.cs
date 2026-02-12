namespace ColinhoDaCa.Application.DTOs.Cupons;

public class CupomDto
{
    public long Id { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public int Tipo { get; set; }
    public decimal Percentual { get; set; }
    public decimal? ValorFixo { get; set; }
    public decimal? MinimoValorTotal { get; set; }
    public int? MinimoPets { get; set; }
    public int? MinimoDiarias { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool Ativo { get; set; }
}
