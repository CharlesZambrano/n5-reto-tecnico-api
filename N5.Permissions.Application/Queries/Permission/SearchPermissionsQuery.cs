using MediatR;
using N5.Permissions.Domain.Entities;
using System.Collections.Generic;

namespace N5.Permissions.Application.Queries
{
    public class SearchPermissionsQuery : IRequest<IEnumerable<Permission>>
    {
        public string Query { get; }

        public SearchPermissionsQuery(string query)
        {
            Query = query;
        }
    }
}
