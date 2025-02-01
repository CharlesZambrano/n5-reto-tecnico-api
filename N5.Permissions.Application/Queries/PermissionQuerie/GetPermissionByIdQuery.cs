// *? n5-reto-tecnico-api/N5.Permissions.Application/Queries/PermissionQuerie/GetPermissionByIdQuery.cs

using MediatR;
using N5.Permissions.Application.DTOs;

namespace N5.Permissions.Application.Queries.PermissionQuerie
{
    public class GetPermissionByIdQuery : IRequest<PermissionDto?>
    {
        public int Id { get; }

        public GetPermissionByIdQuery(int id)
        {
            Id = id;
        }
    }
}
