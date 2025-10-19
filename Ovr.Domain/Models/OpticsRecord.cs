namespace Ovr.Domain.Models
{
    public class OpticsRecord
    {
        public int RecordId { get; set; }
        public long PersonId { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public int LensId { get; set; }
        public string Prescription { get; set; }
        public int FrameId { get; set; }
        public int StatusId { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
