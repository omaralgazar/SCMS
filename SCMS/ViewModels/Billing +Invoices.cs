using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCMS.ViewModels
{
    public class InvoiceItemVm
    {
        public int InvoiceId { get; set; }
        public int BookingId { get; set; }

        public string PatientName { get; set; } = "";
        public string DoctorName { get; set; } = "";

        public DateTime AppointmentDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public double TotalAmount { get; set; }
        public string Status { get; set; } = "";

        public bool IsPaid { get; set; }
        public string? PaymentMethod { get; set; }
    }

    public class PatientInvoicesVm
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; } = "";

        public double TotalPaid { get; set; }
        public double TotalUnpaid { get; set; }

        public IEnumerable<InvoiceItemVm> Invoices { get; set; }
            = new List<InvoiceItemVm>();
    }

    public class InvoiceFormVm
    {
        [Required]
        public int BookingId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero")]
        public double TotalAmount { get; set; }

        public string Status { get; set; } = "Unpaid";
        // Unpaid | Paid | Cancelled

        public string? PaymentMethod { get; set; }
        // Cash | Visa | Insurance | etc.
    }
}
