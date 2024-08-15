﻿public interface IUnitOfWork : IDisposable
{
    IPermisoRepository Permissions { get; }
    Task<int> SaveChangesAsync();

    ITipoPermisoRepository TipoPermissions { get; }
}
