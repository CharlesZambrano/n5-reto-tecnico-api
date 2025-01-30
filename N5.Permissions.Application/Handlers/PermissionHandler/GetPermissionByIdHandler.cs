// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionHandler/GetPermissionByIdHandler.cs

using MediatR;
using N5.Permissions.Application.Queries.PermissionQuerie;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces;

namespace N5.Permissions.Application.Handlers.PermissionHandler
{
    public class GetPermissionByIdHandler : IRequestHandler<GetPermissionByIdQuery, Permission?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPermissionByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Permission?> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Permissions.GetByIdAsync(request.Id);
        }
    }
}
