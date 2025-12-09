using System;
using System.Collections.Generic;
using SCMS.Models;

namespace SCMS.BL.BLInterfaces
{
    public interface IDoctorService
    {
        Doctor Create(Doctor doctor);
        bool Update(Doctor doctor);
        bool Delete(int doctorId);

        Doctor? GetById(int doctorId);
        List<Doctor> GetAll();
        List<Doctor> GetBySpecialization(string specialization);

        List<Appointment> GetAppointmentsForDoctor(int doctorId, DateTime? from = null, DateTime? to = null);
        double GetAverageRate(int doctorId);
    }
}
