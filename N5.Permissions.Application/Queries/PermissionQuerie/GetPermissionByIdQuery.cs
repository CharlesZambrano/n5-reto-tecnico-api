// *? n5-reto-tecnico-api/N5.Permissions.Application/Queries/Permission/GetPermissionByIdQuery.cs

using MediatR;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.Queries.PermissionQuerie
{
    public class GetPermissionByIdQuery : IRequest<Permission?>
    {
        public int Id { get; }

        public GetPermissionByIdQuery(int id)
        {
            Id = id;
        }
    }
}
