// *? n5-reto-tecnico-api/N5.Permissions.Application/Handlers/PermissionHandler/GetPermissionsHandler.cs

using MediatR;
using N5.Permissions.Application.Queries.PermissionQuerie;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Domain.Interfaces;

namespace N5.Permissions.Application.Handlers.PermissionHandler
{
    public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPermissionsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _unitOfWork.Permissions.GetAllAsync();

            return permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                EmployeeName = p.EmployeeName,
                EmployeeSurname = p.EmployeeSurname,
                PermissionTypeId = p.PermissionTypeId,
                PermissionDate = p.PermissionDate,
                PermissionType = new PermissionTypeDto
                {
                    Id = p.PermissionType.Id,
                    Description = p.PermissionType.Description,
                    Code = p.PermissionType.Code
                }
            }).ToList();
        }
    }
}
