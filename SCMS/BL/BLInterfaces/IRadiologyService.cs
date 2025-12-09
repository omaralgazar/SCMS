using System.Collections.Generic;
using SCMS.Models;

namespace SCMS.BL.BLInterfaces
{
    public interface IRadiologyService
    {
        RadiologyRequest CreateRequest(RadiologyRequest request);
        RadiologyRequest? GetRequestById(int id);
        List<RadiologyRequest> GetRequestsForPatient(int patientId);
        List<RadiologyRequest> GetPendingRequestsForRadiologist(int radiologistId);

        RadiologyResult AddResult(RadiologyResult result);
        RadiologyResult? GetResultById(int id);
        List<RadiologyResult> GetResultsForPatient(int patientId);
    }
}
