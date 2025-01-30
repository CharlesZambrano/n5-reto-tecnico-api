using MediatR;
using N5.Permissions.Application.Commands;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace N5.Permissions.Application.Handlers
{
    public class CreatePermissionHandler : IRequestHandler<CreatePermissionCommand, Permission>
    {
        private readonly IPermissionRepository _repository;
        private readonly IPermissionTypeRepository _permissionTypeRepository;

        public CreatePermissionHandler(IPermissionRepository repository, IPermissionTypeRepository permissionTypeRepository)
        {
            _repository = repository;
            _permissionTypeRepository = permissionTypeRepository;
        }

        public async Task<Permission> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            // Obtener el PermissionType correspondiente
            var permissionType = await _permissionTypeRepository.GetByIdAsync(request.PermissionTypeId);
            if (permissionType == null) throw new ArgumentException("Invalid PermissionType ID");

            var permission = new Permission
            {
                EmployeeName = request.EmployeeName,
                EmployeeSurname = request.EmployeeSurname,
                PermissionTypeId = request.PermissionTypeId,
                PermissionType = permissionType,
                PermissionDate = request.PermissionDate
            };

            await _repository.AddAsync(permission);
            return permission;
        }
    }
}
