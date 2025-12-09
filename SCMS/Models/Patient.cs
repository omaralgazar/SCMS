using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        public string Gender { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; } = null!;

        public int Age { get; set; }

        public string? MedicalHistorySummary { get; set; }

        public User User { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

       
        public ICollection<RadiologyRequest> RadiologyRequests { get; set; } = new List<RadiologyRequest>();

        public ICollection<RadiologyResult> RadiologyResults { get; set; } = new List<RadiologyResult>();
        public ICollection<AppointmentBooking> AppointmentBookings { get; set; } = new List<AppointmentBooking>();

    }
}
