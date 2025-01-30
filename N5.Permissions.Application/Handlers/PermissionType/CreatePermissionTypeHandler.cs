using MediatR;
using N5.Permissions.Application.Commands;
using N5.Permissions.Application.Commands.PermissionType;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces;

namespace N5.Permissions.Application.Handlers
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
                Description = request.Description
            };

            await _unitOfWork.PermissionTypes.AddAsync(permissionType);
            await _unitOfWork.CommitAsync();

            return permissionType;
        }
    }
}
