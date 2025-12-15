using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;

namespace SCMS.BL.BLClasses
{
    public class RadiologyService : IRadiologyService
    {
        private readonly AppDbContext _context;

        public RadiologyService(AppDbContext context)
        {
            _context = context;
        }

        public RadiologyRequest CreateRequest(RadiologyRequest request)
        {
            _context.RadiologyRequests.Add(request);
            _context.SaveChanges();
            return request;
        }

        public RadiologyRequest? GetRequestById(int id)
        {
            return _context.RadiologyRequests
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .Include(r => r.Prescription)
                .Include(r => r.Radiologist)
                .FirstOrDefault(r => r.RequestId == id);
        }

        public List<RadiologyRequest> GetRequestsForPatient(int patientId)
        {
            return _context.RadiologyRequests
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.RequestDate)
                .ToList();
        }

        public List<RadiologyRequest> GetPendingRequestsForRadiologist(int radiologistId)
        {
            return _context.RadiologyRequests
                .Where(r => r.RadiologistId == radiologistId && r.Status == "Pending")
                .OrderBy(r => r.RequestDate)
                .ToList();
        }

        public RadiologyResult AddResult(RadiologyResult result)
        {
            _context.RadiologyResults.Add(result);

            var request = _context.RadiologyRequests.FirstOrDefault(r => r.RequestId == result.RequestId);
            if (request != null)
                request.Status = "Completed";

            _context.SaveChanges();
            return result;
        }

        public RadiologyResult? GetResultById(int id)
        {
            return _context.RadiologyResults
                .Include(r => r.Request)
                    .ThenInclude(req => req.Patient)
                .Include(r => r.Radiologist)
                .FirstOrDefault(r => r.ResultId == id);
        }

        public List<RadiologyResult> GetResultsForPatient(int patientId)
        {
            return _context.RadiologyResults
                .Include(r => r.Request)
                .Where(r => r.Request.PatientId == patientId)
                .OrderByDescending(r => r.ResultDate)
                .ToList();
        }
    }
}
