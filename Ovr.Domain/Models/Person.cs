using System.ComponentModel.DataAnnotations;

namespace Ovr.Domain.Models
{
    public class Person
    {
        [Key]
        public long PersonId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
        [StringLength(100, ErrorMessage = "El apellido paterno no puede exceder los 100 caracteres.")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El apellido materno no puede exceder los 100 caracteres.")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "El género es obligatorio.")]
        public int GenderId { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Person), nameof(ValidateBirthDate))]
        public DateTime? BirthDate { get; set; }

        public static ValidationResult? ValidateBirthDate(DateTime? date, ValidationContext context)
        {
            if (date == null)
                return new ValidationResult("La fecha de nacimiento es obligatoria.");

            if (date > DateTime.Today)
                return new ValidationResult("La fecha de nacimiento no puede ser en el futuro.");

            return ValidationResult.Success;
        }

        [Required(ErrorMessage = "El estado de actividad es obligatorio.")]
        public bool IsActive { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico debe tener un formato válido.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "El número de teléfono debe tener un formato válido.")]
        [StringLength(15, ErrorMessage = "El número de teléfono no puede exceder los 15 caracteres.")]
        public string? PhoneNumber { get; set; }

        // Propiedades calculadas
        [StringLength(200)]
        public string GenderName { get; set; } = string.Empty;

        [StringLength(200)]
        public string ShortName { get; set; } = string.Empty;

        [StringLength(300)]
        public string FullName { get; set; } = string.Empty;

        // Campos de Dirección
        [StringLength(150, ErrorMessage = "La calle y número no pueden exceder los 150 caracteres.")]
        public string? AddressStreetNumber { get; set; }

        [StringLength(150, ErrorMessage = "La colonia no puede exceder los 150 caracteres.")]
        public string? AddressNeighborhood { get; set; }

        [StringLength(150, ErrorMessage = "La ciudad no puede exceder los 150 caracteres.")]
        public string? AddressCity { get; set; }

        [StringLength(150, ErrorMessage = "El estado no puede exceder los 150 caracteres.")]
        public string? AddressState { get; set; }

        [StringLength(10, ErrorMessage = "El código postal no puede exceder los 10 caracteres.")]
        public string? AddressPostalCode { get; set; }
    }
}
