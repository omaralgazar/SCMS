using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Range(1, 5)]
        public int Rate { get; set; }

        public string? FeedbackText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; } = null!;

        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; } = null!;
    }
}
