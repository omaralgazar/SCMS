using SCMS.Models;
using System.Collections.Generic;

namespace SCMS.BL.BLInterfaces
{
    public interface IAppointmentBookingService
    {
        AppointmentBooking? BookAppointment(int appointmentId, int patientId);
        bool CancelBooking(int bookingId, int patientId);

        List<AppointmentBooking> GetBookingsByPatient(int patientId);
        List<AppointmentBooking> GetBookingsForAppointment(int appointmentId);
    }
}
