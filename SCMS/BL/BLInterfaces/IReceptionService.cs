using System.Collections.Generic;
using SCMS.Models;

namespace SCMS.BL.BLInterfaces
{
    public interface IReceptionService
    {
        AppointmentBooking? BookForPatient(int appointmentId, int patientId);
        bool CancelBooking(int bookingId);
        bool MarkArrived(int bookingId);
        bool MarkNoShow(int bookingId);

        List<AppointmentBooking> GetTodayBookingsForDoctor(int doctorId);
    }
}
