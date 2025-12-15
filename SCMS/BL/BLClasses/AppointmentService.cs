using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;

namespace SCMS.BL.BLClasses
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }

        public Appointment? Create(Appointment slot)
        {
            if (slot.Capacity <= 0) return null;
            if (slot.StartTime >= slot.EndTime) return null;

            var startDateTime = slot.AppointmentDate.Date + slot.StartTime;
            if (startDateTime <= DateTime.UtcNow) return null;

            bool overlaps = _context.Appointments.Any(a =>
                a.DoctorId == slot.DoctorId &&
                a.AppointmentDate == slot.AppointmentDate &&
                !(slot.EndTime <= a.StartTime || slot.StartTime >= a.EndTime));

            if (overlaps) return null;

            _context.Appointments.Add(slot);
            _context.SaveChanges();
            return slot;
        }

        public bool Update(Appointment slot)
        {
            var existing = _context.Appointments.FirstOrDefault(a => a.AppointmentId == slot.AppointmentId);
            if (existing == null) return false;

            if (slot.Capacity <= 0) return false;
            if (slot.StartTime >= slot.EndTime) return false;

            var startDateTime = slot.AppointmentDate.Date + slot.StartTime;
            if (startDateTime <= DateTime.UtcNow) return false;

            bool overlaps = _context.Appointments.Any(a =>
                a.AppointmentId != slot.AppointmentId &&
                a.DoctorId == slot.DoctorId &&
                a.AppointmentDate == slot.AppointmentDate &&
                !(slot.EndTime <= a.StartTime || slot.StartTime >= a.EndTime));

            if (overlaps) return false;

            _context.Entry(existing).CurrentValues.SetValues(slot);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int appointmentId)
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

        public Appointment? GetById(int appointmentId)
        {
            return _context.Appointments
                .Include(a => a.Bookings)
                .FirstOrDefault(a => a.AppointmentId == appointmentId);
        }

        public List<Appointment> GetAll()
        {
            return _context.Appointments
                .Include(a => a.Bookings)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToList();
        }

        public List<Appointment> GetAvailable()
        {
            return _context.Appointments
                .Where(a => a.Status == "Available")
                .Include(a => a.Bookings)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToList();
        }

        public List<Appointment> GetByDoctor(int doctorId)
        {
            return _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Bookings)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToList();
        }
    }
}
