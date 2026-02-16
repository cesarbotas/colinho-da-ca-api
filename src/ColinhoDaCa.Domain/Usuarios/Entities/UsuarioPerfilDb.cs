namespace ColinhoDaCa.Domain.Usuarios.Entities;

public class UsuarioPerfilDb
{
    public long UsuarioId { get; protected set; }
    public long PerfilId { get; protected set; }
    
    protected UsuarioPerfilDb()
    {
        UsuarioId = default!;
        PerfilId = default!;
    }
    
    private UsuarioPerfilDb(long usuarioId, long perfilId)
    {
        UsuarioId = usuarioId;
        PerfilId = perfilId;
    }
    
    public static UsuarioPerfilDb Create(long usuarioId, long perfilId)
    {
        return new UsuarioPerfilDb(usuarioId, perfilId);
    }
}
