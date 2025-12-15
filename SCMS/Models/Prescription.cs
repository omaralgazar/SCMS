using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Models
{

    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public string Diagnosis { get; set; } = null!;

        [Required]
        public string Treatment { get; set; } = null!;

        public bool RadiologyRequested { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; } = null!;

        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; } = null!;

        public ICollection<RadiologyRequest> RadiologyRequests { get; set; } = new List<RadiologyRequest>();

        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    }
}
