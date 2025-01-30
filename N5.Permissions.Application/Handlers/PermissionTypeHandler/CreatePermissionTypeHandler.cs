// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionTypeHandler/CreatePermissionTypeHandler.cs

using MediatR;
using N5.Permissions.Application.Commands.PermissionTypeCommand;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces;

namespace N5.Permissions.Application.Handlers.PermissionTypeHandler
{
    public class CreatePermissionTypeHandler : IRequestHandler<CreatePermissionTypeCommand, PermissionType>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePermissionTypeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PermissionType> Handle(CreatePermissionTypeCommand request, CancellationToken cancellationToken)
        {
            var permissionType = new PermissionType
            {
                Description = request.Description,
                Permissions = new List<Permission>()
            };

            await _unitOfWork.PermissionTypes.AddAsync(permissionType);
            await _unitOfWork.CommitAsync();

            return permissionType;
        }
    }
}
