using System.ComponentModel.DataAnnotations;

namespace Ovr.Domain.Models
{
    public class Frame
    {
        public int FrameId { get; set; }

        [Required(ErrorMessage = "El nombre del armazón es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres permitidos.")]
        public string FrameName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
