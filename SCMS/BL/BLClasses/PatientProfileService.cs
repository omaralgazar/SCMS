using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;
using SCMS.ViewModels;

namespace SCMS.BL.BLClasses
{
    public class PatientProfileService : IPatientProfileService
    {
        private readonly AppDbContext _context;

        public PatientProfileService(AppDbContext context)
        {
            _context = context;
        }

        public PatientProfileVm? GetPatientProfileForDoctor(int patientId, int doctorId)
        {
            // 1) نتأكد إن الدكتور فعلاً له علاقة بالمريض (في بينهم حجز)
            bool hasRelation = _context.AppointmentBookings
                .Include(b => b.Appointment)
                .Any(b => b.PatientId == patientId &&
                          b.Appointment.DoctorId == doctorId);

            if (!hasRelation)
                return null;   // الدكتور ده مالوش حق يشوف البروفايل

            // 2) نجيب بيانات المريض مع الـ MedicalRecords
            var patient = _context.Patients
                .Include(p => p.User)
                .Include(p => p.MedicalRecords)
                    .ThenInclude(r => r.RelatedPrescription)
                .Include(p => p.MedicalRecords)
                    .ThenInclude(r => r.RadiologyResult)
                .FirstOrDefault(p => p.PatientId == patientId);

            if (patient == null)
                return null;

            var vm = new PatientProfileVm
            {
                PatientId = patient.PatientId,
                FullName = patient.User.FullName,
                Age = patient.Age,
                Gender = patient.Gender,
                Address = patient.Address,
                MedicalHistorySummary = patient.MedicalHistorySummary,
                Records = patient.MedicalRecords
                    .OrderByDescending(r => r.RecordDate)
                    .Select(r => new PatientProfileRecordVm
                    {
                        RecordId = r.RecordId,
                        RecordDate = r.RecordDate,
                        Description = r.Description,
                        Diagnosis = r.RelatedPrescription?.Diagnosis,
                        Treatment = r.RelatedPrescription?.Treatment,
                        RadiologyTestName = r.RadiologyResult?.Request.TestName,
                        RadiologyStatus = r.RadiologyResult?.Status
                    })
                    .ToList()
            };

            return vm;
        }
    }
}
