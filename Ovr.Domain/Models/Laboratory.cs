using System.ComponentModel.DataAnnotations;

namespace Ovr.Domain.Models
{
    public class Laboratory
    {
        public int LaboratoryId { get; set; }

        [Required(ErrorMessage = "El nombre del laboratorio es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre del laboratorio")]
        public string LaboratoryName { get; set; } = string.Empty;

        [Display(Name = "Activo")]
        public bool IsActive { get; set; } = true;
    }
}
