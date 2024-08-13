using Permission.Domain;
using System.Security;

public interface IPermisoRepository
{
    Task<Permiso> GetByIdAsync(int id);
    Task<IEnumerable<Permiso>> GetAllAsync();
    Task AddAsync(Permiso permission);
    Task UpdateAsync(Permiso permission);
    Task DeleteAsync(int id);
}
