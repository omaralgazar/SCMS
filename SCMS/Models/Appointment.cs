using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public int? CreatedByUserId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public double Price { get; set; }

        [Required]
        public int Capacity { get; set; }

        public int CurrentCount { get; set; } = 0;

        [Required]
        public string Status { get; set; } = "Available";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public DateTime StartDateTime => AppointmentDate.Date + StartTime;

        [NotMapped]
        public DateTime EndDateTime => AppointmentDate.Date + EndTime;

        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; } = null!;

        [ForeignKey(nameof(CreatedByUserId))]
        public User? CreatedByUser { get; set; }

        public ICollection<AppointmentBooking> Bookings { get; set; } = new List<AppointmentBooking>();
    }
}
