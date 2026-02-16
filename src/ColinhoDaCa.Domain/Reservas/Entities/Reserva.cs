using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Reservas.Enums;

namespace ColinhoDaCa.Domain.Reservas.Entities;

public class Reserva
{
    private List<ReservaStatusHistorico> _statusHistorico;

    public long Id { get; protected set; }
    public long ClienteId { get; protected set; }
    public DateTime DataInicial { get; protected set; }
    public DateTime DataFinal { get; protected set; }
    public int QuantidadeDiarias { get; protected set; }
    public int QuantidadePets { get; protected set; }
    public decimal ValorTotal { get; protected set; }
    public decimal ValorDesconto { get; protected set; }
    public decimal ValorFinal { get; protected set; }
    public string Observacoes { get; protected set; }
    public ReservaStatus Status { get; protected set; }
    public long? CupomId { get; protected set; }
    public string? ComprovantePagamento { get; protected set; }
    public DateTime? DataPagamento { get; protected set; }
    public string? ObservacoesPagamento { get; protected set; }
    public DateTime DataInclusao { get; protected set; }
    public DateTime DataAlteracao { get; protected set; }
    public Cliente Cliente { get; protected set; }

    public IReadOnlyList<ReservaStatusHistorico> StatusHistorico => _statusHistorico;

    protected Reserva()
    {
        Id = default!;
        ClienteId = default!;
        DataInicial = default!;
        DataFinal = default!;
        Observacoes = default!;
        Cliente = default!;

        _statusHistorico = new();
    }

    private Reserva(long clienteId, DateTime dataInicial, DateTime dataFinal, int quantidadeDiarias, int quantidadePets, decimal valorTotal, decimal valorDesconto, decimal valorFinal, long? cupomId, string obs)
    {
        var now = DateTime.Now;
        
        ClienteId = clienteId;
        DataInicial = dataInicial;
        DataFinal = dataFinal;
        QuantidadeDiarias = quantidadeDiarias;
        QuantidadePets = quantidadePets;
        ValorTotal = valorTotal;
        ValorDesconto = valorDesconto;
        ValorFinal = valorFinal;
        CupomId = cupomId;
        Observacoes = obs ?? throw new ArgumentNullException(nameof(obs));
        Status = ReservaStatus.ReservaCriada;
        DataInclusao = now;
        DataAlteracao = now;

        _statusHistorico = new();
    }

    public static Reserva Create(long clienteId, DateTime dataInicial, DateTime dataFinal, int quantidadeDiarias, int quantidadePets, decimal valorTotal, decimal valorDesconto, decimal valorFinal, long? cupomId, string obs, List<long> petIds, long usuarioId)
    {
        var reserva = new Reserva(clienteId, dataInicial, dataFinal, quantidadeDiarias, quantidadePets, valorTotal, valorDesconto, valorFinal, cupomId, obs);
        return reserva;
    }

    public void Alterar(long clienteId, DateTime dataInicial, DateTime dataFinal, int quantidadeDiarias, int quantidadePets, decimal valorTotal, decimal valorDesconto, decimal valorFinal, long? cupomId, string obs, List<long> petIds)
    {
        ClienteId = clienteId;
        DataInicial = dataInicial;
        DataFinal = dataFinal;
        QuantidadeDiarias = quantidadeDiarias;
        QuantidadePets = quantidadePets;
        ValorTotal = valorTotal;
        ValorDesconto = valorDesconto;
        ValorFinal = valorFinal;
        CupomId = cupomId;
        Observacoes = obs;
        DataAlteracao = DateTime.Now;
    }

    public void ConfirmarReserva(long usuarioId)
    {
        Status = ReservaStatus.ReservaConfirmada;
        DataAlteracao = DateTime.Now;
        _statusHistorico.Add(ReservaStatusHistorico.Create(Id, ReservaStatus.ReservaConfirmada, usuarioId));
    }

    public void AlterarParaPagamentoPendente(long usuarioId)
    {
        Status = ReservaStatus.PagamentoPendente;
        DataAlteracao = DateTime.Now;
        _statusHistorico.Add(ReservaStatusHistorico.Create(Id, ReservaStatus.PagamentoPendente, usuarioId));
    }

    public void EnviarComprovantePagamento(string comprovante, string observacoes)
    {
        ComprovantePagamento = comprovante;
        ObservacoesPagamento = observacoes;
        DataPagamento = DateTime.Now;
        DataAlteracao = DateTime.Now;
    }

    public void AprovarPagamento(long usuarioId)
    {
        Status = ReservaStatus.PagamentoAprovado;
        DataAlteracao = DateTime.Now;
        _statusHistorico.Add(ReservaStatusHistorico.Create(Id, ReservaStatus.PagamentoAprovado, usuarioId));
    }

    public void FinalizarReserva(long usuarioId)
    {
        Status = ReservaStatus.ReservaFinalizada;
        DataAlteracao = DateTime.Now;
        _statusHistorico.Add(ReservaStatusHistorico.Create(Id, ReservaStatus.ReservaFinalizada, usuarioId));
    }

    public void AdicionarStatusHistorico(ReservaStatusHistorico statusHistorico)
    {
        _statusHistorico.Add(statusHistorico);
    }

    public void ConcederDesconto(decimal valorDesconto)
    {
        ValorDesconto = valorDesconto;
        ValorFinal = ValorTotal - ValorDesconto;
        DataAlteracao = DateTime.Now;
    }

    public void AplicarCupom(long cupomId, decimal valorDesconto)
    {
        CupomId = cupomId;
        ValorDesconto = valorDesconto;
        ValorFinal = ValorTotal - ValorDesconto;
        DataAlteracao = DateTime.Now;
    }

    public void CancelarReserva(long usuarioId)
    {
        Status = ReservaStatus.ReservaCancelada;
        DataAlteracao = DateTime.Now;
        _statusHistorico.Add(ReservaStatusHistorico.Create(Id, ReservaStatus.ReservaCancelada, usuarioId));
    }
}
