using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;

namespace SCMS.BL.BLClasses
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public Appointment CreateAppointmentSlot(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
            return appointment;
        }

        public bool UpdateAppointmentSlot(Appointment appointment)
        {
            var existing = _context.Appointments.FirstOrDefault(a => a.AppointmentId == appointment.AppointmentId);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(appointment);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteAppointmentSlot(int appointmentId)
        {
            var slot = _context.Appointments
                .Include(a => a.Bookings)
                .FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (slot == null) return false;
            if (slot.Bookings.Any()) return false;

            _context.Appointments.Remove(slot);
            _context.SaveChanges();
            return true;
        }

        public bool ActivateUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            user.IsActive = true;
            _context.SaveChanges();
            return true;
        }

        public bool DeactivateUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            user.IsActive = false;
            _context.SaveChanges();
            return true;
        }

        public List<Invoice> GetAllInvoices()
        {
            return _context.Invoices
                .Include(i => i.AppointmentBooking)
                    .ThenInclude(b => b.Appointment)
                .OrderByDescending(i => i.CreatedAt)
                .ToList();
        }
    }
}
