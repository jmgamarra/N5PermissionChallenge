using Permission.Domain;
using System.Security;

public interface ITipoPermisoRepository
{
    Task<TipoPermiso> GetByIdAsync(int id);
    Task<IEnumerable<TipoPermiso>> GetAllAsync();

}
