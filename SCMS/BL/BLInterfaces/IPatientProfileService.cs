using SCMS.ViewModels;

namespace SCMS.BL.BLInterfaces
{
    public interface IPatientProfileService
    {
        PatientProfileVm? GetPatientProfileForDoctor(int patientId, int doctorId);
    }
}
