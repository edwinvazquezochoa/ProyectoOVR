using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ovr.Domain.Models
{
    public class SalesNoteDetail
    {
        public long SalesNoteDetailId { get; set; }

        [Required]
        public long SalesNoteId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal PaymentAmount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        [Required]
        public long CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedBy { get; set; }
    }
}