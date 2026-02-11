using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Clientes.Entities;

namespace ColinhoDaCa.Domain.Clientes.Repositories;

public interface IClienteRepository : IRepository<ClienteDb>
{
    Task<ClienteDb> GetByCpfAsync(string cpf);
    Task<ClienteDb> GetByEmailAsync(string email);
}