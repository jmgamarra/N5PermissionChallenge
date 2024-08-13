using MediatR;
using Permission.Domain;

public class GetPermisoByIdQuery : IRequest<Permiso>
{
    public int Id { get; set; }
}
