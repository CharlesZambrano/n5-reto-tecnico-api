using MediatR;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.Commands.PermissionType
{
    public class CreatePermissionTypeCommand : IRequest<PermissionType>
    {
        public string Description { get; set; }

        public CreatePermissionTypeCommand(string description)
        {
            Description = description;
        }
    }
}
