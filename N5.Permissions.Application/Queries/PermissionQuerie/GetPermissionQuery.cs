// *? n5-reto-tecnico-api/N5.Permissions.Application/Queries/Permission/GetPermissionQuery.cs

using MediatR;
using N5.Permissions.Application.DTOs;

namespace N5.Permissions.Application.Queries.PermissionQuerie
{
    public class GetPermissionsQuery : IRequest<IEnumerable<PermissionDto>> { }
}
