using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCMS.Models
{
    public class Receptionist : Staff
    {
        [Required]
        public string Shift { get; set; } = null!;

        public ICollection<ChatThread> ChatThreads { get; set; } = new List<ChatThread>();
    }
}
