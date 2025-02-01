// *? n5-reto-tecnico-api/N5.Permissions.Infrastructure/Elasticsearch/Models/EsPermissionDoc.cs

namespace N5.Permissions.Infrastructure.Elasticsearch.Models
{
    public class EsPermissionDoc
    {
        public int Id { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeSurname { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }
        public string? PermissionTypeDescription { get; set; }
        public string? PermissionTypeCode { get; set; }
    }
}
