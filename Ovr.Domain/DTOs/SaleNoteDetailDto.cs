namespace Ovr.Domain.DTOs
{
    public class SaleNoteDetailDto
    {
        public long SaleNoteId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }

        // Cliente / Paciente
        public long PacienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string ClienteTelefono { get; set; } = string.Empty;
        public string ClienteEmail { get; set; } = string.Empty;
        public string AddressStreetNumber { get; set; } = string.Empty;
        public string AddressNeighborhood { get; set; } = string.Empty;
        public string AddressCity { get; set; } = string.Empty;
        public string AddressState { get; set; } = string.Empty;
        public string AddressPostalCode { get; set; } = string.Empty;

        // Info general
        public long BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public DateTime? CommitmentDate { get; set; }

        // Productos / Servicios
        public string Products_Services { get; set; } = string.Empty;

        // Adaptación Lente
        public bool IsPatientLenses { get; set; }
        public string LensAdaptationNotes { get; set; } = string.Empty;

        // Adaptación Armazón
        public bool IsPatientFrame { get; set; }
        public string FrameAdaptationNotes { get; set; } = string.Empty;

        // Adaptación Lente Contacto
        public string ContactLensAdaptationNotes { get; set; } = string.Empty;

        // Graduación
        public bool IsPatientPrescription { get; set; }
        public string OD_Sphere { get; set; } = string.Empty;
        public string OI_Sphere { get; set; } = string.Empty;
        public string OD_Cylinder { get; set; } = string.Empty;
        public string OI_Cylinder { get; set; } = string.Empty;
        public string OD_Axis { get; set; } = string.Empty;
        public string OI_Axis { get; set; } = string.Empty;
        public string OD_Addition { get; set; } = string.Empty;
        public string OI_Addition { get; set; } = string.Empty;
        public string OD_Prism { get; set; } = string.Empty;
        public string OI_Prism { get; set; } = string.Empty;
        public string OD_DNP { get; set; } = string.Empty;
        public string OI_DNP { get; set; } = string.Empty;
        public string DIP_Far { get; set; } = string.Empty;
        public string DIP_Near { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public bool IsBifocal { get; set; }
        public bool IsProgressive { get; set; }
        public string VertexDistance { get; set; } = string.Empty;
        public string PantoscopicAngle { get; set; } = string.Empty;
        public string PanoramicAngle { get; set; } = string.Empty;

        // Modalidad de pago
        public string PaymentMethod { get; set; } = string.Empty;
        public int? CreditMonths { get; set; }
        public string BankOrigin { get; set; } = string.Empty;
        public string BankDestination { get; set; } = string.Empty;

        // Importes
        public decimal? Amount_Frame { get; set; }
        public decimal? Amount_Lens { get; set; }
        public decimal? Amount_Accessories { get; set; }
        public decimal? Amount_Service { get; set; }
        public decimal Subtotal { get; set; }
        public decimal? Discount { get; set; }
        public decimal Total { get; set; }

        // Listas relacionadas
        public List<SaleNoteAccessoryDto> Accessories { get; set; } = new();
        public List<SaleNotePaymentDto> Payments { get; set; } = new();
    }

    public class SaleNoteAccessoryDto
    {
        public long SaleAccessoryId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public long SaleNoteId { get; set; } // ✅ agrega esta
    }

    public class SaleNotePaymentDto
    {
        public long SalePaymentId { get; set; }
        public long SaleNoteId { get; set; } // ✅ requerido para el TVP
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal? RemainingBalance { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
