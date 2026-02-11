namespace ColinhoDaCa.Application.DTOs.Reservas;

public class ReservasDto
{
    public long Id { get; set; }
    public long ClienteId { get; set; }
    public string ClienteNome { get; set; }
    public DateTime DataInicial { get; set; }
    public DateTime DataFinal { get; set; }
    public int QuantidadeDiarias { get; set; }
    public int QuantidadePets { get; set; }
    public decimal ValorTotal { get; set; }
    public string Observacoes { get; set; }
    public int Status { get; set; }
    public string? ComprovantePagamento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string? ObservacoesPagamento { get; set; }
    public Dictionary<int, bool> StatusTimeline { get; set; }
    public List<StatusHistoricoDto> Historico { get; set; }
    public List<PetReservaDto> Pets { get; set; }
}

public class StatusHistoricoDto
{
    public int Status { get; set; }
    public long UsuarioId { get; set; }
    public string UsuarioNome { get; set; }
    public DateTime DataAlteracao { get; set; }
}

public class PetReservaDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
}
