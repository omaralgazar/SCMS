using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;

namespace SCMS.BL.BLClasses
{
    public class PatientService : IPatientService
    {
        private readonly AppDbContext _context;

        public PatientService(AppDbContext context)
        {
            _context = context;
        }

        public Patient Create(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
            return patient;
        }

        public bool Update(Patient patient)
        {
            if (!_context.Patients.Any(p => p.UserId == patient.UserId))
                return false;

            _context.Patients.Update(patient);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.UserId == id);
            if (patient == null) return false;

            _context.Patients.Remove(patient);
            _context.SaveChanges();
            return true;
        }

        public Patient? GetById(int id)
        {
            return _context.Patients
                .Include(p => p.AppointmentBookings)
                .FirstOrDefault(p => p.UserId == id);
        }

        public List<Patient> GetByName(string name)
        {
            return _context.Patients
                .Where(p => p.FullName.Contains(name))
                .ToList();
        }

        public List<Patient> GetAll()
        {
            return _context.Patients.ToList();
        }
    }
}
