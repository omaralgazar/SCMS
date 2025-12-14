using System;
using System.ComponentModel.DataAnnotations;

namespace SCMS.Models
{
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Action { get; set; } = string.Empty;

        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
