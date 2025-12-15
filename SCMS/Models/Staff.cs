using System.ComponentModel.DataAnnotations;

namespace SCMS.Models
{
    public class Staff : User
    {
        [Required]
        public string DepartmentName { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        public double Salary { get; set; }
    }
}
