// *? n5-reto-tecnico-api/N5.Permissions.Application/Commands/UpdatePermissionCommand.cs

using MediatR;

namespace N5.Permissions.Application.Commands.Permission
{
    public class UpdatePermissionCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }

        public UpdatePermissionCommand(int id, string employeeName, string employeeSurname, int permissionTypeId, DateTime permissionDate)
        {
            Id = id;
            EmployeeName = employeeName;
            EmployeeSurname = employeeSurname;
            PermissionTypeId = permissionTypeId;
            PermissionDate = permissionDate;
        }
    }
}
