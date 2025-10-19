using System.ComponentModel.DataAnnotations;

namespace Ovr.Domain.Models
{
    public class Gender
    {
        [Key] // Define la clave primaria
        public int GenderId { get; set; } // Identificador único del género

        [Required(ErrorMessage = "El nombre del género es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre del género no puede exceder los 50 caracteres.")]
        public string GenderName { get; set; } = string.Empty; // Nombre del género

    }
}
