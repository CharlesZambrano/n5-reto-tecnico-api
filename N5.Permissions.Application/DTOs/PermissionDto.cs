// *? n5-reto-tecnico-api/N5.Permissions.Application/DTOs/PermissionDto.cs

namespace N5.Permissions.Application.DTOs
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }
        public PermissionTypeDto PermissionType { get; set; }
    }
}
