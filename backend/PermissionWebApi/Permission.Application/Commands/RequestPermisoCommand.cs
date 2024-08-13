using MediatR;
using Permission.Domain;

public class RequestPermisoCommand : IRequest<Permiso>
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public DateTime Fecha { get; set; }
    public int TipoId { get; set; }
}
