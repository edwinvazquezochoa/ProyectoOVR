using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ovr.Domain.Models
{
    public class SalesNote
    {
        public long SalesNoteId { get; set; }

        [Required]
        public long BranchId { get; set; }

        public string? SaleFolio { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        public long PatientId { get; set; }

        [Required]
        public int LensId { get; set; }

        [MaxLength(255)]
        public string? LensDescription { get; set; }

        [Required]
        public int FrameId { get; set; }

        [MaxLength(255)]
        public string? FrameDescription { get; set; }

        [Required]
        public int StatusId { get; set; }

        public DateTime? CommitmentDate { get; set; }

        [Required]
        public int LaboratoryId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0.")]
        public decimal TotalAmount { get; set; }

        public DateTime? CreatedAt { get; set; }

        [Required]
        public long CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
