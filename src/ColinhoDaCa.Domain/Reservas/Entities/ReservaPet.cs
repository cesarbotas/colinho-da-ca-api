using ColinhoDaCa.Domain.Pets.Entities;

namespace ColinhoDaCa.Domain.Reservas.Entities;

public class ReservaPet
{
    public long ReservaId { get; protected set; }
    public long PetId { get; protected set; }

    public Reserva Reserva { get; protected set; }
    public Pet Pet { get; protected set; }

    protected ReservaPet()
    {
        ReservaId = default!;
        PetId = default!;
    }
    
    private ReservaPet(long reservaId, long petId)
    {
        ReservaId = reservaId;
        PetId = petId;
    }
    
    public static ReservaPet Create(long reservaId, long petId)
    {
        return new ReservaPet(reservaId, petId);
    }
}