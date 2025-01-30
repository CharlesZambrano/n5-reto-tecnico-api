// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/GetPermissionByIdHandler.cs

using MediatR;
using N5.Permissions.Application.Queries;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces.Repositories;

namespace N5.Permissions.Application.Handlers
{
    public class GetPermissionByIdHandler : IRequestHandler<GetPermissionByIdQuery, Permission?>
    {
        private readonly IPermissionRepository _repository;

        public GetPermissionByIdHandler(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Permission?> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id);
        }
    }
}
