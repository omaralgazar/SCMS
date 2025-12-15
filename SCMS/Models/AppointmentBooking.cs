using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Models
{
    public class AppointmentBooking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int OrderNumber { get; set; }

        [Required]
        public string Status { get; set; } = "Booked";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(AppointmentId))]
        public Appointment Appointment { get; set; } = null!;

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; } = null!;

        public Invoice? Invoice { get; set; }
    }
}
