using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCMS.Models
{
    public class Doctor : Staff
    {
        [Required]
        public string Specialization { get; set; } = null!;

        public int YearsOfExperience { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
