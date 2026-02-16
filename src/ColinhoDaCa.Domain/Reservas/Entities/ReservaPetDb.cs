namespace ColinhoDaCa.Domain.Reservas.Entities;

public class ReservaPetDb
{
    public long ReservaId { get; protected set; }
    public long PetId { get; protected set; }
    
    protected ReservaPetDb()
    {
        ReservaId = default!;
        PetId = default!;
    }
    
    private ReservaPetDb(long reservaId, long petId)
    {
        ReservaId = reservaId;
        PetId = petId;
    }
    
    public static ReservaPetDb Create(long reservaId, long petId)
    {
        return new ReservaPetDb(reservaId, petId);
    }
}
