using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Usuarios.Entities;

namespace ColinhoDaCa.Domain.Usuarios.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario> GetByClienteIdAsync(long clienteId);
    Task<Usuario> GetByClienteIdWithPerfisAsync(long clienteId);
    Task<List<PerfilUsuarioDto>> GetPerfisUsuarioAsync(long usuarioId);
}

public class PerfilUsuarioDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
}