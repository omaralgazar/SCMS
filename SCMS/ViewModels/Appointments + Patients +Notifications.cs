using System;
using System.Collections.Generic;

namespace SCMS.ViewModels
{
    public class AppointmentSummaryVm
    {
        public int AppointmentId { get; set; }

        public string DoctorName { get; set; } = "";
        public string PatientName { get; set; } = "";   // لو في حجز واحد/عرض مختصر

        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public double Price { get; set; }

        public int Capacity { get; set; }
        public int CurrentCount { get; set; }

        public string Status { get; set; } = "";

        public int? OrderNumber { get; set; } // دور المريض (لو هتعرضه للمريض/الريسبشن)
    }

    public class PatientSummaryVm
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = "";
        public int Age { get; set; }
        public string Phone { get; set; } = "";
        public DateTime? LastVisit { get; set; }
    }

    public class NotificationVm
    {
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? LinkAction { get; set; }
        public string? LinkController { get; set; }
        public int? RelatedId { get; set; }
    }
}
