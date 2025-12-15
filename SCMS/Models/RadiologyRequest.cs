using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Models
{
    public class RadiologyRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public int? PrescriptionId { get; set; }

        public int? RadiologistId { get; set; }

        [Required]
        public string TestName { get; set; } = null!;

        public string? ClinicalNotes { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; } = null!;

        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; } = null!;

        [ForeignKey(nameof(PrescriptionId))]
        public Prescription? Prescription { get; set; }

        [ForeignKey(nameof(RadiologistId))]
        public Radiologist? Radiologist { get; set; }

        public RadiologyResult? Result { get; set; }
    }
}
