using ColinhoDaCa.Domain.Usuarios.Entities;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using ColinhoDaCa.Infra.Data._Shared.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.Infra.Data.Context.Repositories.Usuarios;

public class UsuarioRepository : Repository<UsuarioDb>, IUsuarioRepository
{
    private readonly ColinhoDaCaContext _context;

    public UsuarioRepository(ColinhoDaCaContext context) 
        : base(context)
    {
        _context = context;
    }

    public async Task<UsuarioDb> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}