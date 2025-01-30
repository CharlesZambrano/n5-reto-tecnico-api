// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/GetPermissionsHandler.cs

using MediatR;
using N5.Permissions.Application.Queries;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces.Repositories;

namespace N5.Permissions.Application.Handlers
{
    public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<Permission>>
    {
        private readonly IPermissionRepository _repository;

        public GetPermissionsHandler(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Permission>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
