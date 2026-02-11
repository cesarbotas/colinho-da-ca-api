using ColinhoDaCa.Domain.Reservas.Enums;

namespace ColinhoDaCa.Domain.Reservas.Entities;

public class ReservaStatusHistoricoDb
{
    public long Id { get; set; }
    public long ReservaId { get; set; }
    public ReservaStatus Status { get; set; }
    public long UsuarioId { get; set; }
    public DateTime DataAlteracao { get; set; }

    public static ReservaStatusHistoricoDb Create(long reservaId, ReservaStatus status, long usuarioId)
    {
        return new ReservaStatusHistoricoDb
        {
            ReservaId = reservaId,
            Status = status,
            UsuarioId = usuarioId,
            DataAlteracao = DateTime.Now
        };
    }
}
