using System.Collections.Generic;
using SCMS.Models;

namespace SCMS.BL.BLInterfaces
{
    public interface IFeedbackService
    {
        Feedback AddFeedback(Feedback feedback);
        List<Feedback> GetFeedbacksForDoctor(int doctorId);
        double GetAverageRateForDoctor(int doctorId);
    }
}
