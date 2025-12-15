using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public double TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Not Billed yet";

        [ForeignKey(nameof(BookingId))]
        public AppointmentBooking AppointmentBooking { get; set; } = null!;
    }
}
