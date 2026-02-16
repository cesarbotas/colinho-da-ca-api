using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Clientes.Entities;

namespace ColinhoDaCa.Domain.Clientes.Repositories;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<Cliente> GetByCpfAsync(string cpf);
    Task<Cliente> GetByEmailAsync(string email);
}