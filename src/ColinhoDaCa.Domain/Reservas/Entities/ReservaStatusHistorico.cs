using ColinhoDaCa.Domain.Reservas.Enums;
using ColinhoDaCa.Domain.Usuarios.Entities;

namespace ColinhoDaCa.Domain.Reservas.Entities;

public class ReservaStatusHistorico
{
    public long Id { get; protected set; }
    public long ReservaId { get; protected set; }    
    public ReservaStatus Status { get; protected set; }
    public long UsuarioId { get; protected set; }
    public DateTime DataAlteracao { get; protected set; }
    public Reserva Reserva { get; protected set; }
    public Usuario Usuario { get; protected set; }

    protected ReservaStatusHistorico()
    {
        Id = default!;
        ReservaId = default!;
        UsuarioId = default!;
    }
    
    private ReservaStatusHistorico(long reservaId, ReservaStatus status, long usuarioId)
    {
        ReservaId = reservaId;
        Status = status;
        UsuarioId = usuarioId;
        DataAlteracao = DateTime.Now;
    }

    public static ReservaStatusHistorico Create(long reservaId, ReservaStatus status, long usuarioId)
    {
        return new ReservaStatusHistorico(reservaId, status, usuarioId);
    }
}