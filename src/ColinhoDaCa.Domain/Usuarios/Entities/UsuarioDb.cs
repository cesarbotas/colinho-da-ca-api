namespace ColinhoDaCa.Domain.Usuarios.Entities;

public class UsuarioDb
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string SenhaHash { get; set; }
    public long ClienteId { get; set; }
    public DateTime DataInclusao { get; set; }
    public DateTime DataAlteracao { get; set; }

    public UsuarioDb()
    {
        
    }

    public static UsuarioDb Create(string nome, string email, string senhaHash)
    {
        var now = DateTime.Now;

        return new UsuarioDb
        {
            Nome = nome,
            Email = email,
            SenhaHash = senhaHash,
            DataInclusao = now,
            DataAlteracao = now
        };
    }

    public void VincularCliente(long clienteId)
    {
        ClienteId = clienteId;
        DataAlteracao = DateTime.Now;
    }

    public void AlterarSenha(string senhaHash)
    {
        SenhaHash = senhaHash;
        DataAlteracao = DateTime.Now;
    }
}