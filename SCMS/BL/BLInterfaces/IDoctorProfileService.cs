using SCMS.ViewModels;

namespace SCMS.BL.BLInterfaces
{
    public interface IDoctorProfileService
    {
        DoctorProfileVm? GetProfile(int doctorId);
    }
}
