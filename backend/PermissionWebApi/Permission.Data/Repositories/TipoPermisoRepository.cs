using Microsoft.EntityFrameworkCore;
using System.Security;
using Permission.Domain;

public class TipoPermisoRepository : ITipoPermisoRepository
{
    private readonly PermissionsDbContext _context;

    public TipoPermisoRepository(PermissionsDbContext context)
    {
        _context = context;
    }

    public async Task<TipoPermiso> GetByIdAsync(int id)
    {
        return await _context.TiposPermiso.FindAsync(id);
    }

    public async Task<IEnumerable<TipoPermiso>> GetAllAsync()
    {
        return await _context.TiposPermiso.ToListAsync();
    }
}
