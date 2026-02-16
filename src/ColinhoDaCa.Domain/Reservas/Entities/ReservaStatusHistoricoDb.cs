using ColinhoDaCa.Domain.Reservas.Enums;

namespace ColinhoDaCa.Domain.Reservas.Entities;

public class ReservaStatusHistoricoDb
{
    public long Id { get; protected set; }
    public long ReservaId { get; protected set; }
    public ReservaStatus Status { get; protected set; }
    public long UsuarioId { get; protected set; }
    public DateTime DataAlteracao { get; protected set; }
    
    protected ReservaStatusHistoricoDb()
    {
        Id = default!;
        ReservaId = default!;
        UsuarioId = default!;
    }
    
    private ReservaStatusHistoricoDb(long reservaId, ReservaStatus status, long usuarioId)
    {
        ReservaId = reservaId;
        Status = status;
        UsuarioId = usuarioId;
        DataAlteracao = DateTime.Now;
    }

    public static ReservaStatusHistoricoDb Create(long reservaId, ReservaStatus status, long usuarioId)
    {
        return new ReservaStatusHistoricoDb(reservaId, status, usuarioId);
    }
}
