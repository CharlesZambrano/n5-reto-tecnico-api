// *? n5-reto-tecnico-api/N5.Permissions.Application/Commands/PermissionTypeCommand/CreatePermissionTypeCommand.cs

using MediatR;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.Commands.PermissionTypeCommand
{
    public class CreatePermissionTypeCommand : IRequest<PermissionType>
    {
        public required string Description { get; set; }
    }
}
