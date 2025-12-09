using SCMS.Models;

namespace SCMS.BL.BLInterfaces
{
    public interface IAdminService
    {
        Appointment CreateAppointmentSlot(Appointment appointment);
        bool UpdateAppointmentSlot(Appointment appointment);
        bool DeleteAppointmentSlot(int appointmentId);

        bool ActivateUser(int userId);
        bool DeactivateUser(int userId);

        List<Invoice> GetAllInvoices();
       
    }

}
