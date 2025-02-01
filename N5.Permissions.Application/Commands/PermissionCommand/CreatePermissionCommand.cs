// *? n5-reto-tecnico-api/N5.Permissions.Application/Commands/PermissionCommand/CreatePermissionCommand.cs

using MediatR;
using N5.Permissions.Application.DTOs;

namespace N5.Permissions.Application.Commands.PermissionCommand
{
    public class CreatePermissionCommand : IRequest<PermissionDto>
    {
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }

        public CreatePermissionCommand(string employeeName, string employeeSurname, int permissionTypeId, DateTime permissionDate)
        {
            EmployeeName = employeeName;
            EmployeeSurname = employeeSurname;
            PermissionTypeId = permissionTypeId;
            PermissionDate = permissionDate;
        }
    }
}
