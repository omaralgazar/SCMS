using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;

namespace SCMS.BL.BLClasses
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext _context;

        public DoctorService(AppDbContext context)
        {
            _context = context;
        }

        public Doctor Create(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
            return doctor;
        }

        public bool Update(Doctor doctor)
        {
            if (!_context.Doctors.Any(d => d.UserId == doctor.UserId))
                return false;

            _context.Doctors.Update(doctor);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int doctorId)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.UserId == doctorId);
            if (doctor == null) return false;

            bool hasRelations =
                _context.Appointments.Any(a => a.DoctorId == doctorId) ||
                _context.Prescriptions.Any(p => p.DoctorId == doctorId) ||
                _context.Feedbacks.Any(f => f.DoctorId == doctorId) ||
                _context.RadiologyRequests.Any(r => r.DoctorId == doctorId);

            if (hasRelations) return false;

            _context.Doctors.Remove(doctor);
            _context.SaveChanges();
            return true;
        }

        public Doctor? GetById(int doctorId)
        {
            return _context.Doctors.FirstOrDefault(d => d.UserId == doctorId);
        }

        public List<Doctor> GetAll()
        {
            return _context.Doctors.ToList();
        }

        public List<Doctor> GetBySpecialization(string specialization)
        {
            return _context.Doctors
                .Where(d => d.Specialization == specialization)
                .ToList();
        }

        public List<Appointment> GetAppointmentsForDoctor(int doctorId, DateTime? from = null, DateTime? to = null)
        {
            var query = _context.Appointments.Where(a => a.DoctorId == doctorId);

            if (from.HasValue) query = query.Where(a => a.AppointmentDate >= from.Value.Date);
            if (to.HasValue) query = query.Where(a => a.AppointmentDate <= to.Value.Date);

            return query
                .Include(a => a.Bookings)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToList();
        }

        public double GetAverageRate(int doctorId)
        {
            var q = _context.Feedbacks.Where(f => f.DoctorId == doctorId);
            if (!q.Any()) return 0;
            return q.Average(f => f.Rate);
        }
    }
}
