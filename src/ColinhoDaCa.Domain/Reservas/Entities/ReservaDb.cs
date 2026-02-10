namespace ColinhoDaCa.Domain.Reservas.Entities;

public class ReservaDb
{
    public long Id { get; set; }
    public long ClienteId { get; set; }
    public DateTime DataInicial { get; set; }
    public DateTime DataFinal { get; set; }
    public string Observacoes { get; set; }
    public DateTime DataInclusao { get; set; }
    public DateTime DataAlteracao { get; set; }
    public List<ReservaPetDb> ReservaPets { get; set; }

    public ReservaDb()
    {
        ReservaPets = new List<ReservaPetDb>();
    }

    public static ReservaDb Create(long clienteId, DateTime dataInicial, DateTime dataFinal, string obs, List<long> petIds)
    {
        var now = DateTime.Now;

        var reserva = new ReservaDb
        {
            ClienteId = clienteId,
            DataInicial = dataInicial,
            DataFinal = dataFinal,
            Observacoes = obs,
            DataInclusao = now,
            DataAlteracao = now
        };
        
        foreach (var petId in petIds)
        {
            reserva.ReservaPets.Add(new ReservaPetDb { PetId = petId });
        }
        
        return reserva;
    }

    public void Alterar(long clienteId, DateTime dataInicial, DateTime dataFinal, string obs, List<long> petIds)
    {
        ClienteId = clienteId;
        DataInicial = dataInicial;
        DataFinal = dataFinal;
        Observacoes = obs;
        DataAlteracao = DateTime.Now;
        
        ReservaPets.Clear();

        foreach (var petId in petIds)
        {
            ReservaPets.Add(new ReservaPetDb { PetId = petId, ReservaId = Id });
        }
    }
}
