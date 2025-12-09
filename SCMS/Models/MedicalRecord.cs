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
        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }

        [ForeignKey(nameof(RadiologyResult))]
        public int? RadiologyResultId { get; set; }

        [ForeignKey(nameof(RelatedPrescription))]
        public int? PrescriptionId { get; set; }

        public string? Description { get; set; }
       

        public DateTime RecordDate { get; set; } = DateTime.UtcNow;

        public Patient Patient { get; set; } = null!;
        public Prescription? RelatedPrescription { get; set; }
        public RadiologyResult? RadiologyResult { get; set; }
    }
}
