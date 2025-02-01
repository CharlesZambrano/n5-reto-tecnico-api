// *? n5-reto-tecnico-api/N5.Permissions.Application/Commands/PermissionTypeCommand/CreatePermissionTypeCommand.cs

using MediatR;
using N5.Permissions.Application.DTOs;

namespace N5.Permissions.Application.Commands.PermissionTypeCommand
{
    public class CreatePermissionTypeCommand : IRequest<PermissionTypeDto>
    {
        public required string Code { get; set; }
        public required string Description { get; set; }
    }
}
