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
            var doctor = _context.Doctors.FirstOrDefault(d => d.UserId == doctorId);
            if (doctor == null) return null;

            var feedbackQuery = _context.Feedbacks.Where(f => f.DoctorId == doctorId);

            double averageRate = feedbackQuery.Any() ? feedbackQuery.Average(f => f.Rate) : 0;
            int feedbackCount = feedbackQuery.Count();

            var upcomingAppointments = _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate >= DateTime.UtcNow.Date)
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

            return new DoctorProfileVm
            {
                DoctorId = doctor.UserId,
                FullName = doctor.FullName,
                Specialization = doctor.Specialization,
                YearsOfExperience = doctor.YearsOfExperience,
                DepartmentName = doctor.DepartmentName,
                PhoneNumber = doctor.PhoneNumber,
                AverageRate = averageRate,
                FeedbackCount = feedbackCount,
                UpcomingAppointments = upcomingAppointments
            };
        }
    }
}
