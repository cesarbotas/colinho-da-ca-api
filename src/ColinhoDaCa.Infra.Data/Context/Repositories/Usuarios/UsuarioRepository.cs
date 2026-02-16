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

    public async Task<UsuarioDb> GetByClienteIdAsync(long clienteId)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.ClienteId == clienteId);
    }

    public async Task<UsuarioDb> GetByClienteIdWithPerfisAsync(long clienteId)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.ClienteId == clienteId);
    }

    public async Task<List<PerfilUsuarioDto>> GetPerfisUsuarioAsync(long usuarioId)
    {
        return await _context.UsuarioPerfis
            .Where(up => up.UsuarioId == usuarioId)
            .Join(_context.Perfis,
                up => up.PerfilId,
                p => p.Id,
                (up, p) => new PerfilUsuarioDto
                {
                    Id = p.Id,
                    Nome = p.Nome
                })
            .ToListAsync();
    }
}