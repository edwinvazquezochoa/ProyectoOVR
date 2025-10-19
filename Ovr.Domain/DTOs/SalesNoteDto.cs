using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ovr.Domain.DTOs
{
    public class SalesNoteDto
    {
        public long SalesNoteId { get; set; }
        public string SaleFolio { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string LensDescription { get; set; } = string.Empty;
        public string FrameDescription { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
