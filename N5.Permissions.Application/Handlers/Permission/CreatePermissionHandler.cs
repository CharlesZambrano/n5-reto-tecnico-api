// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/CreatePermissionHandler.cs

using MediatR;
using N5.Permissions.Application.Commands.Permission;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces;
using N5.Permissions.Infrastructure.Elasticsearch.Services;

namespace N5.Permissions.Application.Handlers.Permission
{
    public class CreatePermissionHandler : IRequestHandler<CreatePermissionCommand, Permission>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ElasticsearchService _elasticsearchService;

        public CreatePermissionHandler(IUnitOfWork unitOfWork, ElasticsearchService elasticsearchService)
        {
            _unitOfWork = unitOfWork;
            _elasticsearchService = elasticsearchService;
        }

        public async Task<Permission> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permissionType = await _unitOfWork.PermissionTypes.GetByIdAsync(request.PermissionTypeId);
            if (permissionType == null) throw new ArgumentException("Invalid PermissionType ID");

            var permission = new Permission
            {
                EmployeeName = request.EmployeeName,
                EmployeeSurname = request.EmployeeSurname,
                PermissionTypeId = request.PermissionTypeId,
                PermissionType = permissionType,
                PermissionDate = request.PermissionDate
            };

            _unitOfWork.Permissions.AddAsync(permission);
            await _unitOfWork.CommitAsync();

            // Indexar en Elasticsearch
            await _elasticsearchService.IndexPermissionAsync(permission);

            return permission;
        }
    }
}
