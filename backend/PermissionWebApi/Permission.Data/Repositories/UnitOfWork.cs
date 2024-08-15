public class UnitOfWork : IUnitOfWork
{
    private readonly PermissionsDbContext _context;
    private IPermisoRepository _permissions;
    private ITipoPermisoRepository _tipopermissions;

    public UnitOfWork(PermissionsDbContext context)
    {
        _context = context;
    }

    public IPermisoRepository Permissions
        => _permissions ??= new PermisoRepository(_context);
    public ITipoPermisoRepository TipoPermissions
       => _tipopermissions ??= new TipoPermisoRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
