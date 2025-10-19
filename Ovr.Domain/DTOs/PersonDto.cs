using System.ComponentModel.DataAnnotations;

namespace Ovr.Domain.DTOs
{
    public class PersonDto
    {
        public long PersonId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? MiddleName { get; set; }

        [Required]
        public int GenderId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        // Propiedades calculadas
        public string GenderName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        // Propiedades de Dirección (nuevas)
        [StringLength(150)]
        public string? AddressStreetNumber { get; set; }

        [StringLength(150)]
        public string? AddressNeighborhood { get; set; }

        [StringLength(150)]
        public string? AddressCity { get; set; }

        [StringLength(150)]
        public string? AddressState { get; set; }

        public string? AddressPostalCode { get; set; } // lo dejo string porque en Persons es int pero en pantalla lo muestras como texto
    }
}
