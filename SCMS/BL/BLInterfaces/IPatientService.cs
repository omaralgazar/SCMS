using SCMS.Models;
using System.Collections.Generic;

namespace SCMS.BL.BLInterfaces
{
    public interface IPatientService
    {
        Patient Create(Patient patient);
        bool Update(Patient patient);
        bool Delete(int id);
        Patient? GetById(int id);
        List<Patient> GetByName(string name);
        List<Patient> GetAll();
    }
}
