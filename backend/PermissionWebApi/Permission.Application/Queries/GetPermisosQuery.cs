using MediatR;
using Permission.Domain;

public class GetPermisosQuery : IRequest<IEnumerable<Permiso>>
{
}
