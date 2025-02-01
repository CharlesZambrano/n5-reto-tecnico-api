// *? n5-reto-tecnico-api/N5.Permissions.Application/Queries/PermissionType/GetPermissionTypesQuery.cs

using MediatR;
using N5.Permissions.Application.DTOs;

namespace N5.Permissions.Application.Queries.PermissionTypeQuerie
{
    public class GetPermissionTypesQuery : IRequest<IEnumerable<PermissionTypeDto>> { }
}
