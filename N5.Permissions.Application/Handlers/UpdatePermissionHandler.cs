// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/UpdatePermissionHandler.cs

using MediatR;
using N5.Permissions.Application.Commands;
using N5.Permissions.Domain.Interfaces.Repositories;
using N5.Permissions.Infrastructure.Elasticsearch.Services;

namespace N5.Permissions.Application.Handlers
{
    public class UpdatePermissionHandler : IRequestHandler<UpdatePermissionCommand, bool>
    {
        private readonly IPermissionRepository _repository;
        private readonly IPermissionTypeRepository _permissionTypeRepository;
        private readonly ElasticsearchService _elasticsearchService;

        public UpdatePermissionHandler(IPermissionRepository repository, IPermissionTypeRepository permissionTypeRepository, ElasticsearchService elasticsearchService)
        {
            _repository = repository;
            _permissionTypeRepository = permissionTypeRepository;
            _elasticsearchService = elasticsearchService;
        }

        public async Task<bool> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await _repository.GetByIdAsync(request.Id);
            if (permission == null) return false;

            var permissionType = await _permissionTypeRepository.GetByIdAsync(request.PermissionTypeId);
            if (permissionType == null) throw new ArgumentException("Invalid PermissionType ID");

            permission.EmployeeName = request.EmployeeName;
            permission.EmployeeSurname = request.EmployeeSurname;
            permission.PermissionTypeId = request.PermissionTypeId;
            permission.PermissionType = permissionType;
            permission.PermissionDate = request.PermissionDate;

            await _repository.UpdateAsync(permission);

            // Actualizar en Elasticsearch
            await _elasticsearchService.IndexPermissionAsync(permission);

            return true;
        }
    }
}
