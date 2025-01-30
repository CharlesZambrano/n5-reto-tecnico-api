using MediatR;
using N5.Permissions.Domain.Entities;
using System.Collections.Generic;

namespace N5.Permissions.Application.Queries
{
    public class GetPermissionsQuery : IRequest<IEnumerable<Permission>> { }
}
