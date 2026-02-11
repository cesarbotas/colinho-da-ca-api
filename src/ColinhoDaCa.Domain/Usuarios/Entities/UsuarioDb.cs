namespace ColinhoDaCa.Domain.Usuarios.Entities;

public class UsuarioDb
{
    public long Id { get; set; }
    public string SenhaHash { get; set; }
    public long ClienteId { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataInclusao { get; set; }
    public DateTime DataAlteracao { get; set; }
    public List<UsuarioPerfilDb> UsuarioPerfis { get; set; }

    public UsuarioDb()
    {
        UsuarioPerfis = new List<UsuarioPerfilDb>();
    }

    public static UsuarioDb Create(string senhaHash, long clienteId)
    {
        var now = DateTime.Now;

        return new UsuarioDb
        {
            SenhaHash = senhaHash,
            ClienteId = clienteId,
            Ativo = true,
            DataInclusao = now,
            DataAlteracao = now
        };
    }

    public void AlterarSenha(string senhaHash)
    {
        SenhaHash = senhaHash;
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
}