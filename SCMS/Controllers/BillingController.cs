using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMS.Models;
using SCMS.ViewModels;

namespace SCMS.Controllers
{
    public class BillingController : Controller
    {
        private readonly AppDbContext _context;

        public BillingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PatientInvoices(int patientUserId)
        {
            var patient = await _context.Set<Patient>()
                .FirstOrDefaultAsync(p => p.UserId == patientUserId);

            if (patient == null)
                return NotFound();

            var invoices = await _context.Invoices
                .Include(i => i.AppointmentBooking)
                    .ThenInclude(b => b.Appointment)
                        .ThenInclude(a => a.Doctor)
                .Where(i => i.AppointmentBooking.PatientId == patientUserId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            var items = invoices.Select(i => new InvoiceItemVm
            {
                InvoiceId = i.InvoiceId,
                BookingId = i.BookingId,
                PatientName = patient.FullName,
                DoctorName = i.AppointmentBooking.Appointment.Doctor.FullName,
                AppointmentDate = i.AppointmentBooking.Appointment.AppointmentDate,
                TotalAmount = i.TotalAmount,
                Status = i.Status
            }).ToList();

            var vm = new PatientInvoicesVm
            {
                PatientId = patientUserId,
                PatientName = patient.FullName,
                TotalPaid = items.Where(x => x.Status == "Paid").Sum(x => x.TotalAmount),
                TotalUnpaid = items.Where(x => x.Status != "Paid").Sum(x => x.TotalAmount),
                Invoices = items
            };

            return View(vm);
        }
    }
}
