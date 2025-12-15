using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;

namespace SCMS.BL.BLClasses
{
    public class FeedbackService : IFeedbackService
    {
        private readonly AppDbContext _context;

        public FeedbackService(AppDbContext context)
        {
            _context = context;
        }

        public Feedback AddFeedback(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
            return feedback;
        }

        public List<Feedback> GetFeedbacksForDoctor(int doctorId)
        {
            return _context.Feedbacks
                .Where(f => f.DoctorId == doctorId)
                .OrderByDescending(f => f.CreatedAt)
                .Include(f => f.Patient)
                .ToList();
        }

        public double GetAverageRateForDoctor(int doctorId)
        {
            var query = _context.Feedbacks.Where(f => f.DoctorId == doctorId);
            if (!query.Any()) return 0;
            return query.Average(f => f.Rate);
        }
    }
}
