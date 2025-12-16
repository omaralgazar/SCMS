using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMS.Models;
using SCMS.ViewModels.Patient;

namespace SCMS.Controllers
{
    public class PatientController : Controller
    {
        private readonly AppDbContext _context;

        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        private async Task<Patient?> GetPatientByUserId(int userId)
        {
            return await _context.Patients
                .Include(p => p.AppointmentBookings)
                .Include(p => p.Prescriptions)
                .Include(p => p.MedicalRecords)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        private async Task<PatientVM?> GetPatientVM(int userId)
        {
            var patient = await GetPatientByUserId(userId);
            if (patient == null) return null;

            return new PatientVM
            {
                UserId = userId,

                FullName = patient.FullName,
                Gender = patient.Gender,
                DateOfBirth = patient.DateOfBirth,
                Age = patient.Age,
                Address = patient.Address,
                MedicalHistorySummary = patient.MedicalHistorySummary,

                AppointmentsCount = patient.AppointmentBookings.Count,
                PrescriptionsCount = patient.Prescriptions.Count,
                MedicalRecordsCount = patient.MedicalRecords.Count
            };
        }

        // /Patient/Dashboard/13   (13 = UserId)
        public async Task<IActionResult> Dashboard(int id)
        {
            var vm = await GetPatientVM(id);
            if (vm == null) return NotFound("Patient not found for this user");
            return View(vm);
        }

        // /Patient/Profile/13   (13 = UserId)
        public async Task<IActionResult> Profile(int id)
        {
            var vm = await GetPatientVM(id);
            if (vm == null) return NotFound("Patient not found for this user");
            return View(vm);
        }

        // ✅ /Patient/Appointments/13
        public async Task<IActionResult> Appointments(int id)
        {
            var vm = await GetPatientVM(id);
            if (vm == null) return NotFound("Patient not found for this user");
            return View(vm);
        }

        // ✅ /Patient/Prescriptions/13
        public async Task<IActionResult> Prescriptions(int id)
        {
            var vm = await GetPatientVM(id);
            if (vm == null) return NotFound("Patient not found for this user");
            return View(vm);
        }

        // ✅ /Patient/MedicalRecords/13
        public async Task<IActionResult> MedicalRecords(int id)
        {
            var vm = await GetPatientVM(id);
            if (vm == null) return NotFound("Patient not found for this user");
            return View(vm);
        }
    }
}
