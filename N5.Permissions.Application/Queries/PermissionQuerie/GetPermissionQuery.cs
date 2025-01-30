// *? n5-reto-tecnico-api/N5.Permissions.Application/Queries/Permission/GetPermissionQuery.cs

using MediatR;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.Queries.PermissionQuerie
{
    public class GetPermissionsQuery : IRequest<IEnumerable<Permission>> { }
}
