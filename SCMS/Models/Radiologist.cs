using System.Collections.Generic;

namespace SCMS.Models
{
    public class Radiologist : Staff
    {
        public ICollection<RadiologyRequest> RadiologyRequests { get; set; } = new List<RadiologyRequest>();
        public ICollection<RadiologyResult> RadiologyResults { get; set; } = new List<RadiologyResult>();
    }
}
