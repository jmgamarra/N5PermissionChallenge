using Microsoft.EntityFrameworkCore;
using System.Security;
using Permission.Domain;

public class PermisoRepository : IPermisoRepository
{
    private readonly PermissionsDbContext _context;

    public PermisoRepository(PermissionsDbContext context)
    {
        _context = context;
    }

    public async Task<Permiso> GetByIdAsync(int id)
    {
        return await _context.Permisos.FindAsync(id);
    }

    public async Task<IEnumerable<Permiso>> GetAllAsync()
    {
        return await _context.Permisos.ToListAsync();
    }

    public async Task AddAsync(Permiso permission)
    {
        await _context.Permisos.AddAsync(permission);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Permiso permission)
    {
        _context.Permisos.Update(permission);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var permission = await _context.Permisos.FindAsync(id);
        if (permission != null)
        {
            _context.Permisos.Remove(permission);
            await _context.SaveChangesAsync();
        }
    }
}
