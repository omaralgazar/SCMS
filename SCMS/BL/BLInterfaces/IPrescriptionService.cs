using System.Collections.Generic;
using SCMS.Models;

namespace SCMS.BL.BLInterfaces
{
    public interface IPrescriptionService
    {
        Prescription Create(Prescription prescription);
        bool Update(Prescription prescription);
        Prescription? GetById(int id);
        List<Prescription> GetForPatient(int patientId);
        List<Prescription> GetForDoctor(int doctorId);
    }
}
