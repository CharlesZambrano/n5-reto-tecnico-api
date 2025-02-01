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

        [Required]
        [MaxLength(3)]
        public required string Code { get; set; }

        public required ICollection<Permission> Permissions { get; set; }
    }
}