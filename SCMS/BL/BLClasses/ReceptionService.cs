using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;

namespace SCMS.BL.BLClasses
{
    public class ReceptionService : IReceptionService
    {
        private readonly AppDbContext _context;

        public ReceptionService(AppDbContext context)
        {
            _context = context;
        }

        public AppointmentBooking? BookForPatient(int appointmentId, int patientId)
        {
            var appointment = _context.Appointments
                .Include(a => a.Bookings)
                .FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null) return null;
            if (appointment.Status != "Available") return null;

            var startDateTime = appointment.AppointmentDate.Date + appointment.StartTime;
            if (startDateTime <= DateTime.UtcNow) return null;

            if (appointment.CurrentCount >= appointment.Capacity)
            {
                appointment.Status = "Full";
                _context.SaveChanges();
                return null;
            }

            bool alreadyBooked = _context.AppointmentBookings
                .Any(b => b.AppointmentId == appointmentId && b.PatientId == patientId && b.Status == "Booked");

            if (alreadyBooked) return null;

            var booking = new AppointmentBooking
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                OrderNumber = appointment.CurrentCount + 1,
                Status = "Booked",
                CreatedAt = DateTime.UtcNow
            };

            appointment.CurrentCount++;
            if (appointment.CurrentCount >= appointment.Capacity)
                appointment.Status = "Full";

            _context.AppointmentBookings.Add(booking);
            _context.SaveChanges();
            return booking;
        }

        public bool CancelBooking(int bookingId)
        {
            var booking = _context.AppointmentBookings
                .Include(b => b.Appointment)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null) return false;
            if (booking.Status == "Cancelled" || booking.Status == "NoShow") return false;

            var appointment = booking.Appointment;

            if (appointment.CurrentCount > 0)
                appointment.CurrentCount--;

            if (appointment.Status == "Full" && appointment.CurrentCount < appointment.Capacity)
                appointment.Status = "Available";

            booking.Status = "Cancelled";
            _context.SaveChanges();
            return true;
        }

        public bool MarkArrived(int bookingId)
        {
            var booking = _context.AppointmentBookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return false;
            if (booking.Status != "Booked") return false;

            booking.Status = "Arrived";
            _context.SaveChanges();
            return true;
        }

        public bool MarkNoShow(int bookingId)
        {
            var booking = _context.AppointmentBookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return false;
            if (booking.Status != "Booked") return false;

            booking.Status = "NoShow";
            _context.SaveChanges();
            return true;
        }

        public List<AppointmentBooking> GetTodayBookingsForDoctor(int doctorId)
        {
            var today = DateTime.UtcNow.Date;

            return _context.AppointmentBookings
                .Include(b => b.Appointment)
                .Include(b => b.Patient)
                .Where(b => b.Appointment.DoctorId == doctorId &&
                            b.Appointment.AppointmentDate == today)
                .OrderBy(b => b.Appointment.StartTime)
                .ThenBy(b => b.OrderNumber)
                .ToList();
        }
    }
}
