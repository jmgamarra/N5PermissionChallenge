using MediatR;
using Permission.Domain;

public class ModifyPermisoCommand : IRequest<Permiso>
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public DateTime Fecha { get; set; }
    public int TipoId { get; set; }
}
