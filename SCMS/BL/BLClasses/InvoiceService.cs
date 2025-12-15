using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;

namespace SCMS.BL.BLClasses
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;

        public InvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public Invoice? CreateForBooking(int bookingId)
        {
            var booking = _context.AppointmentBookings
                .Include(b => b.Appointment)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null) return null;
            if (_context.Invoices.Any(i => i.BookingId == bookingId)) return null;
            if (booking.Status != "Booked") return null;

            var invoice = new Invoice
            {
                BookingId = bookingId,
                TotalAmount = booking.Appointment.Price,
                Status = "Not Billed yet"
            };

            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            return invoice;
        }

        public Invoice? GetById(int invoiceId)
        {
            return _context.Invoices
                .Include(i => i.AppointmentBooking)
                    .ThenInclude(b => b.Appointment)
                .FirstOrDefault(i => i.InvoiceId == invoiceId);
        }

        public Invoice? GetByBooking(int bookingId)
        {
            return _context.Invoices
                .Include(i => i.AppointmentBooking)
                    .ThenInclude(b => b.Appointment)
                .FirstOrDefault(i => i.BookingId == bookingId);
        }

        public List<Invoice> GetByPatient(int patientId)
        {
            return _context.Invoices
                .Include(i => i.AppointmentBooking)
                    .ThenInclude(b => b.Appointment)
                .Where(i => i.AppointmentBooking.PatientId == patientId)
                .OrderByDescending(i => i.CreatedAt)
                .ToList();
        }

        public bool MarkAsPaid(int invoiceId)
        {
            var invoice = _context.Invoices.FirstOrDefault(i => i.InvoiceId == invoiceId);
            if (invoice == null) return false;

            invoice.Status = "Paid";
            _context.SaveChanges();
            return true;
        }
    }
}
