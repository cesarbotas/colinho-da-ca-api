using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Usuarios.Entities;

namespace ColinhoDaCa.Domain.Usuarios.Repositories;

public interface IUsuarioRepository : IRepository<UsuarioDb>
{
    Task<UsuarioDb> GetByClienteIdAsync(long clienteId);
}