using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMS.Models;
using SCMS.ViewModels;

namespace SCMS.Controllers
{
    public class DoctorController : Controller
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard(int? doctorId)
        {
            var id = doctorId ?? 0;
            if (id == 0) return NotFound();

            var doctor = await _context.Set<Doctor>()
                .Include(d => d.Feedbacks)
                .Include(d => d.Appointments)
                    .ThenInclude(a => a.Bookings)
                    .ThenInclude(b => b.Patient)
                .FirstOrDefaultAsync(d => d.UserId == id);

            if (doctor == null) return NotFound();

            var vm = new DoctorProfileVm
            {
                DoctorId = doctor.UserId,
                FullName = doctor.FullName,
                Specialization = doctor.Specialization,
                YearsOfExperience = doctor.YearsOfExperience,
                DepartmentName = doctor.DepartmentName,
                PhoneNumber = doctor.PhoneNumber,
                AverageRate = doctor.Feedbacks.Any() ? doctor.Feedbacks.Average(f => f.Rate) : 0,
                FeedbackCount = doctor.Feedbacks.Count,
                UpcomingAppointments = doctor.Appointments
                    .Where(a => a.AppointmentDate >= DateTime.Today)
                    .OrderBy(a => a.AppointmentDate)
                    .Select(a => new DoctorAppointmentVm
                    {
                        AppointmentId = a.AppointmentId,
                        AppointmentDate = a.AppointmentDate,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        Capacity = a.Capacity,
                        CurrentCount = a.CurrentCount,
                        Status = a.Status
                    }).ToList()
            };

            return View(vm);
        }
    }
}
