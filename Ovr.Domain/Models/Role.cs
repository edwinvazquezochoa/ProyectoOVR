using System.ComponentModel.DataAnnotations;

namespace Ovr.Domain.Models
{
    public class Role
    {
        [Key] // Define la clave primaria
        public int RoleId { get; set; } // Identificador único del rol

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del rol no puede exceder los 100 caracteres.")]
        public string RoleName { get; set; } = string.Empty; // Nombre del rol
        public bool IsActive { get; set; }
    }
}
