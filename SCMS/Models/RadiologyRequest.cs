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
        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }

        [Required]
        [ForeignKey(nameof(Doctor))]
        public int DoctorId { get; set; }

        [ForeignKey(nameof(Prescription))]
        public int? PrescriptionId { get; set; }

        [ForeignKey(nameof(Radiologist))]
        public int? RadiologistId { get; set; }

        [Required]
        public string TestName { get; set; } = null!;

        public string? ClinicalNotes { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public Prescription? Prescription { get; set; }
        public Radiologist? Radiologist { get; set; }

        public RadiologyResult? Result { get; set; }
    }
}
