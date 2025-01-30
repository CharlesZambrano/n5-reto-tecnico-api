// *? n5-reto-tecnico-api/N5.Permissions.Domain/Entities/Permission.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace N5.Permissions.Domain.Entities
{
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string EmployeeName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string EmployeeSurname { get; set; }

        [Required]
        public required int PermissionTypeId { get; set; } // Clave foránea

        [Required]
        public required DateTime PermissionDate { get; set; }

        // Relación con PermissionType
        [ForeignKey("PermissionTypeId")]
        public required PermissionType PermissionType { get; set; }
    }
}
