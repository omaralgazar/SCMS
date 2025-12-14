using System.Collections.Generic;
using SCMS.Models;

namespace SCMS.ViewModels
{
    public class ReportsVm
    {
        public int TotalUsers { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
        public int TotalAppointments { get; set; }

        public List<Appointment> RecentAppointments { get; set; } = new List<Appointment>();
    }
}
