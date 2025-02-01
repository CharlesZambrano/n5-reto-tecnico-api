// *? n5-reto-tecnico-api/N5.Permissions.Application/Queries/Permission/SearchPermissionsQuery.cs

using MediatR;
using N5.Permissions.Application.DTOs;

namespace N5.Permissions.Application.Queries.PermissionQuerie
{
    public class SearchPermissionsQuery : IRequest<IEnumerable<PermissionDto>>
    {
        public string Query { get; }

        public SearchPermissionsQuery(string query)
        {
            Query = query;
        }
    }
}
