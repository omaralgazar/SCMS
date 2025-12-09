using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;

namespace SCMS.BL.BLClasses
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly AppDbContext _context;

        public PrescriptionService(AppDbContext context)
        {
            _context = context;
        }

        public Prescription Create(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            _context.SaveChanges();
            return prescription;
        }

        public bool Update(Prescription prescription)
        {
            if (!_context.Prescriptions.Any(p => p.PrescriptionId == prescription.PrescriptionId))
                return false;

            _context.Prescriptions.Update(prescription);
            _context.SaveChanges();
            return true;
        }

        public Prescription? GetById(int id)
        {
            return _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .FirstOrDefault(p => p.PrescriptionId == id);
        }

        public List<Prescription> GetForPatient(int patientId)
        {
            return _context.Prescriptions
                .Where(p => p.PatientId == patientId)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }

        public List<Prescription> GetForDoctor(int doctorId)
        {
            return _context.Prescriptions
                .Where(p => p.DoctorId == doctorId)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }
    }
}
