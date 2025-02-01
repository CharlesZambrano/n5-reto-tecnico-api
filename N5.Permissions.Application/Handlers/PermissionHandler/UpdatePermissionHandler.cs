// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionHandler/UpdatePermissionHandler.cs

using MediatR;
using N5.Permissions.Application.Commands.PermissionCommand;
using N5.Permissions.Domain.Interfaces;
using N5.Permissions.Infrastructure.Elasticsearch.Services;
using System.ComponentModel.DataAnnotations;

namespace N5.Permissions.Application.Handlers.PermissionHandler
{
    public class UpdatePermissionHandler : IRequestHandler<UpdatePermissionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ElasticsearchService _elasticsearchService;

        public UpdatePermissionHandler(IUnitOfWork unitOfWork, ElasticsearchService elasticsearchService)
        {
            _unitOfWork = unitOfWork;
            _elasticsearchService = elasticsearchService;
        }

        public async Task<bool> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            // Validaciones en el handler
            if (string.IsNullOrWhiteSpace(request.EmployeeName))
                throw new ValidationException("Employee name is required.");
            if (string.IsNullOrWhiteSpace(request.EmployeeSurname))
                throw new ValidationException("Employee surname is required.");

            // Validación: PermissionDate debe ser solo fecha (sin hora)
            if (request.PermissionDate.TimeOfDay != TimeSpan.Zero)
                throw new ValidationException("PermissionDate must be in the format yyyy-MM-dd (time must be 00:00:00).");

            var permission = await _unitOfWork.Permissions.GetByIdAsync(request.Id);
            if (permission == null) return false;

            var permissionType = await _unitOfWork.PermissionTypes.GetByIdAsync(request.PermissionTypeId);
            if (permissionType == null)
                throw new ArgumentException("Invalid PermissionType ID");

            permission.EmployeeName = request.EmployeeName;
            permission.EmployeeSurname = request.EmployeeSurname;
            permission.PermissionTypeId = request.PermissionTypeId;
            permission.PermissionType = permissionType;
            permission.PermissionDate = request.PermissionDate;

            await _unitOfWork.Permissions.UpdateAsync(permission);
            await _unitOfWork.CommitAsync();

            await _elasticsearchService.IndexPermissionAsync(permission);

            return true;
        }
    }
}
