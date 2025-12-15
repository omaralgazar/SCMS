using System.ComponentModel.DataAnnotations;

namespace SCMS.Models
{
    public class Admin : User
    {
        [Required]
        public string AccessLevel { get; set; } = "Full";
    }
}
