using MediatR;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.Commands
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
