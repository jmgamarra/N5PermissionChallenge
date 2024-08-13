using MediatR;
using Permission.Domain;

public class GetPermisoByIdQueryHandler : IRequestHandler<GetPermisoByIdQuery, Permiso>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPermisoByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Permiso> Handle(GetPermisoByIdQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Permissions.GetByIdAsync(request.Id);
    }
}
