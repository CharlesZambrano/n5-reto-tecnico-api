// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/CreatePermissionHandler.cs

using MediatR;
using N5.Permissions.Application.Commands;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces.Repositories;
using N5.Permissions.Infrastructure.Elasticsearch.Services;


namespace N5.Permissions.Application.Handlers
{
    public class CreatePermissionHandler : IRequestHandler<CreatePermissionCommand, Permission>
    {
        private readonly IPermissionRepository _repository;
        private readonly IPermissionTypeRepository _permissionTypeRepository;
        private readonly ElasticsearchService _elasticsearchService;

        public CreatePermissionHandler(IPermissionRepository repository, IPermissionTypeRepository permissionTypeRepository, ElasticsearchService elasticsearchService)
        {
            _repository = repository;
            _permissionTypeRepository = permissionTypeRepository;
            _elasticsearchService = elasticsearchService;
        }

        public async Task<Permission> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
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

            // Indexar en Elasticsearch
            await _elasticsearchService.IndexPermissionAsync(permission);

            return permission;
        }
    }
}
