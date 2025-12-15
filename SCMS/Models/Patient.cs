using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCMS.Models
{
    public class Patient : User
    {
        [Required]
        public string Gender { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; } = null!;

        public int Age { get; set; }

        public string? MedicalHistorySummary { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<RadiologyRequest> RadiologyRequests { get; set; } = new List<RadiologyRequest>();
        public ICollection<AppointmentBooking> AppointmentBookings { get; set; } = new List<AppointmentBooking>();
        public ICollection<ChatThread> ChatThreads { get; set; } = new List<ChatThread>();
    }
}
