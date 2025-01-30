// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionTypeHandler/GetPermissionTypesHandler.cs

using MediatR;
using N5.Permissions.Application.Queries.PermissionTypeQuerie;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces;

namespace N5.Permissions.Application.Handlers.PermissionHandler
{
    public class GetPermissionTypesHandler : IRequestHandler<GetPermissionTypesQuery, IEnumerable<PermissionType>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPermissionTypesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PermissionType>> Handle(GetPermissionTypesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.PermissionTypes.GetAllAsync();
        }
    }
}
