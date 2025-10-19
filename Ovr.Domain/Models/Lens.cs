using System.ComponentModel.DataAnnotations;

namespace Ovr.Domain.Models
{
    public class Len
    {
        public int LensId { get; set; }

        [Required(ErrorMessage = "El nombre del lente es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres permitidos.")]
        public string LensName { get; set; } = string.Empty;
    }
}
