public class UnitOfWork : IUnitOfWork
{
    private readonly PermissionsDbContext _context;
    private IPermisoRepository _permissions;

    public UnitOfWork(PermissionsDbContext context)
    {
        _context = context;
    }

    public IPermisoRepository Permissions
        => _permissions ??= new PermisoRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
