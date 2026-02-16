namespace ColinhoDaCa.Domain.Usuarios.Entities;

public class UsuarioDb
{
    private List<UsuarioPerfilDb> _usuarioPerfis;

    public long Id { get; protected set; }
    public string SenhaHash { get; protected set; }
    public long ClienteId { get; protected set; }
    public bool Ativo { get; protected set; }
    public DateTime DataInclusao { get; protected set; }
    public DateTime DataAlteracao { get; protected set; }
    
    public IReadOnlyList<UsuarioPerfilDb> UsuarioPerfis => _usuarioPerfis;

    protected UsuarioDb()
    {
        Id = default!;
        SenhaHash = default!;
        ClienteId = default!;
        
        _usuarioPerfis = new();
    }

    private UsuarioDb(string senhaHash, long clienteId)
    {
        var now = DateTime.Now;
        
        SenhaHash = senhaHash ?? throw new ArgumentNullException(nameof(senhaHash));
        ClienteId = clienteId;
        Ativo = true;
        DataInclusao = now;
        DataAlteracao = now;
        
        _usuarioPerfis = new();
    }

    public static UsuarioDb Create(string senhaHash, long clienteId)
    {
        return new UsuarioDb(senhaHash, clienteId);
    }

    public void AlterarSenha(string senhaHash)
    {
        SenhaHash = senhaHash ?? throw new ArgumentNullException(nameof(senhaHash));
        DataAlteracao = DateTime.Now;
    }

    public void Ativar()
    {
        Ativo = true;
        DataAlteracao = DateTime.Now;
    }

    public void Desativar()
    {
        Ativo = false;
        DataAlteracao = DateTime.Now;
    }

    public void AdicionarPerfil(UsuarioPerfilDb usuarioPerfil)
    {
        _usuarioPerfis.Add(usuarioPerfil);
    }

    public void RemoverPerfil(long perfilId)
    {
        var perfil = _usuarioPerfis.FirstOrDefault(up => up.PerfilId == perfilId);
        if (perfil != null)
        {
            _usuarioPerfis.Remove(perfil);
        }
    }
}