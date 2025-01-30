// *? n5-reto-tecnico-api/N5.Permissions.Domain/Entities/PermissionType.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace N5.Permissions.Domain.Entities
{
    public class PermissionType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Description { get; set; }

        // Relación con Permissions (1 tipo puede tener muchos permisos)
        public required ICollection<Permission> Permissions { get; set; }
    }
}
