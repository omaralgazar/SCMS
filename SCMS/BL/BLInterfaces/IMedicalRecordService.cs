using System.Collections.Generic;
using SCMS.Models;

namespace SCMS.BL.BLInterfaces
{
    public interface IMedicalRecordService
    {
        MedicalRecord CreateManualRecord(int patientId, string? description, int? prescriptionId = null, int? radiologyResultId = null);
        MedicalRecord? AddRecordFromPrescription(int prescriptionId, string? description = null);
        MedicalRecord? AddRecordFromRadiologyResult(int radiologyResultId, string? description = null);
        MedicalRecord? GetById(int recordId);
        List<MedicalRecord> GetByPatient(int patientId);
    }
}
