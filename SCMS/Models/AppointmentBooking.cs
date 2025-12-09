using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Models
{
    public class AppointmentBooking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [ForeignKey(nameof(Appointment))]
        public int AppointmentId { get; set; }

        [Required]
        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }

        [Required]
        public int OrderNumber { get; set; }

        [Required]
        public string Status { get; set; } = "Booked";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Appointment Appointment { get; set; } = null!;
        public Patient Patient { get; set; } = null!;

        public Invoice? Invoice { get; set; }
    }
}
