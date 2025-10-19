using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ovr.Domain.Models
{
    public class User
    {
        [Key]
        public long UserId { get; set; }

        [Required(ErrorMessage = "El ID de la persona es obligatorio.")]
        public long PersonId { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre de usuario no puede exceder los 150 caracteres.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico debe tener un formato válido.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [JsonIgnore]
        [StringLength(65, ErrorMessage = "El hash de la contraseña no puede exceder los 65 caracteres.")]
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsPasswordTemp { get; set; } = false;

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "El estado de actividad es obligatorio.")]
        public bool IsActive { get; set; } = false;

        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // ✅ Nuevas propiedades para sucursal
        public long? BranchId { get; set; } // Relación con tabla Branches
        public bool IsGlobal { get; set; } = false; // Si puede ver todas las sucursales
        public string? BrancheName { get; set; } // Nombre de la sucursal (calculado)

        // Propiedades calculadas
        [StringLength(200)]
        public string ShortName { get; set; } = string.Empty;

        [StringLength(300)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty;

        // Verificación
        [StringLength(255)]
        public string? VerificationToken { get; set; }

        public DateTime? TokenExpirationDate { get; set; }
    }
}
