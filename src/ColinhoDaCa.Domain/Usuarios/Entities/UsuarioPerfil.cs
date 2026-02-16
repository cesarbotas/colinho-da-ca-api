namespace ColinhoDaCa.Domain.Usuarios.Entities;

public class UsuarioPerfil
{
    public long UsuarioId { get; protected set; }
    public long PerfilId { get; protected set; }
    
    protected UsuarioPerfil()
    {
        UsuarioId = default!;
        PerfilId = default!;
    }
    
    private UsuarioPerfil(long usuarioId, long perfilId)
    {
        UsuarioId = usuarioId;
        PerfilId = perfilId;
    }
    
    public static UsuarioPerfil Create(long usuarioId, long perfilId)
    {
        return new UsuarioPerfil(usuarioId, perfilId);
    }
}
