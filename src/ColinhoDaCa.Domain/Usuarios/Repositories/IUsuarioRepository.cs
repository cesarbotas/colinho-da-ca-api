using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Usuarios.Entities;

namespace ColinhoDaCa.Domain.Usuarios.Repositories;

public interface IUsuarioRepository : IRepository<UsuarioDb>
{
    Task<UsuarioDb> GetByClienteIdAsync(long clienteId);
    Task<UsuarioDb> GetByClienteIdWithPerfisAsync(long clienteId);
    Task<List<PerfilUsuarioDto>> GetPerfisUsuarioAsync(long usuarioId);
}

public class PerfilUsuarioDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
}