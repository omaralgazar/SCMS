using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Models
{
    public class MedicalRecord
    {
        [Key]
        public int RecordId { get; set; }

        [Required]
        public int PatientId { get; set; }

        public int? RadiologyResultId { get; set; }

        public int? PrescriptionId { get; set; }

        public string? Description { get; set; }

        public DateTime RecordDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; } = null!;

        [ForeignKey(nameof(PrescriptionId))]
        public Prescription? RelatedPrescription { get; set; }

        [ForeignKey(nameof(RadiologyResultId))]
        public RadiologyResult? RadiologyResult { get; set; }
    }
}
