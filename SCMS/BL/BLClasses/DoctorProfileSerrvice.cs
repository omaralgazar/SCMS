using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;
using SCMS.ViewModels;

namespace SCMS.BL.BLClasses
{
    public class DoctorProfileService : IDoctorProfileService
    {
        private readonly AppDbContext _context;

        public DoctorProfileService(AppDbContext context)
        {
            _context = context;
        }

        public DoctorProfileVm? GetProfile(int doctorId)
        {
            var doctor = _context.Doctors
                .Include(d => d.Staff)
                    .ThenInclude(s => s.User)
                .FirstOrDefault(d => d.DoctorId == doctorId);

            if (doctor == null)
                return null;

            var feedbackQuery = _context.Feedbacks
                .Where(f => f.DoctorId == doctorId);

            double averageRate = 0;
            int feedbackCount = 0;

            if (feedbackQuery.Any())
            {
                averageRate = feedbackQuery.Average(f => f.Rate);
                feedbackCount = feedbackQuery.Count();
            }

            var upcomingAppointments = _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.AppointmentDate >= DateTime.UtcNow.Date)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .Select(a => new DoctorAppointmentVm
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Capacity = a.Capacity,
                    CurrentCount = a.CurrentCount,
                    Status = a.Status
                })
                .ToList();

            var vm = new DoctorProfileVm
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.Staff.User.FullName,
                Specialization = doctor.Specialization,
                YearsOfExperience = doctor.YearsOfExperience,
                DepartmentName = doctor.Staff.DepartmentName,
                PhoneNumber = doctor.Staff.PhoneNumber,
                AverageRate = averageRate,
                FeedbackCount = feedbackCount,
                UpcomingAppointments = upcomingAppointments
            };

            return vm;
        }
    }
}
