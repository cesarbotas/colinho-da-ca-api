using ColinhoDaCa.Domain.Reservas.Enums;

namespace ColinhoDaCa.Domain.Reservas.Entities;

public class ReservaDb
{
    public long Id { get; set; }
    public long ClienteId { get; set; }
    public DateTime DataInicial { get; set; }
    public DateTime DataFinal { get; set; }
    public int QuantidadeDiarias { get; set; }
    public int QuantidadePets { get; set; }
    public decimal ValorTotal { get; set; }
    public string Observacoes { get; set; }
    public ReservaStatus Status { get; set; }
    public string? ComprovantePagamento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string? ObservacoesPagamento { get; set; }
    public DateTime DataInclusao { get; set; }
    public DateTime DataAlteracao { get; set; }
    public List<ReservaPetDb> ReservaPets { get; set; }
    public List<ReservaStatusHistoricoDb> StatusHistorico { get; set; }

    public ReservaDb()
    {
        ReservaPets = new List<ReservaPetDb>();
        StatusHistorico = new List<ReservaStatusHistoricoDb>();
    }

    public static ReservaDb Create(long clienteId, DateTime dataInicial, DateTime dataFinal, int quantidadeDiarias, int quantidadePets, decimal valorTotal, string obs, List<long> petIds, long usuarioId)
    {
        var now = DateTime.Now;

        var reserva = new ReservaDb
        {
            ClienteId = clienteId,
            DataInicial = dataInicial,
            DataFinal = dataFinal,
            QuantidadeDiarias = quantidadeDiarias,
            QuantidadePets = quantidadePets,
            ValorTotal = valorTotal,
            Observacoes = obs,
            Status = ReservaStatus.ReservaCriada,
            DataInclusao = now,
            DataAlteracao = now
        };
        
        foreach (var petId in petIds)
        {
            reserva.ReservaPets.Add(new ReservaPetDb { PetId = petId });
        }

        reserva.StatusHistorico.Add(ReservaStatusHistoricoDb.Create(0, ReservaStatus.ReservaCriada, usuarioId));
        
        return reserva;
    }

    public void Alterar(long clienteId, DateTime dataInicial, DateTime dataFinal, int quantidadeDiarias, int quantidadePets, decimal valorTotal, string obs, List<long> petIds)
    {
        ClienteId = clienteId;
        DataInicial = dataInicial;
        DataFinal = dataFinal;
        QuantidadeDiarias = quantidadeDiarias;
        QuantidadePets = quantidadePets;
        ValorTotal = valorTotal;
        Observacoes = obs;
        DataAlteracao = DateTime.Now;
        
        ReservaPets.Clear();

        foreach (var petId in petIds)
        {
            ReservaPets.Add(new ReservaPetDb { PetId = petId, ReservaId = Id });
        }
    }

    public void ConfirmarReserva(long usuarioId)
    {
        Status = ReservaStatus.ReservaConfirmada;
        DataAlteracao = DateTime.Now;
        StatusHistorico.Add(ReservaStatusHistoricoDb.Create(Id, ReservaStatus.ReservaConfirmada, usuarioId));
    }

    public void AlterarParaPagamentoPendente(long usuarioId)
    {
        Status = ReservaStatus.PagamentoPendente;
        DataAlteracao = DateTime.Now;
        StatusHistorico.Add(ReservaStatusHistoricoDb.Create(Id, ReservaStatus.PagamentoPendente, usuarioId));
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
        StatusHistorico.Add(ReservaStatusHistoricoDb.Create(Id, ReservaStatus.PagamentoAprovado, usuarioId));
    }

    public void FinalizarReserva(long usuarioId)
    {
        Status = ReservaStatus.ReservaFinalizada;
        DataAlteracao = DateTime.Now;
        StatusHistorico.Add(ReservaStatusHistoricoDb.Create(Id, ReservaStatus.ReservaFinalizada, usuarioId));
    }
}
