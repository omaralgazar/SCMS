using SCMS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMS.Models
{
    public class ChatThread
    {
        [Key]
        public int ThreadId { get; set; }

        [Required]
        public int PatientId { get; set; }

        public int? ReceptionistId { get; set; }

        public string Status { get; set; } = "Open";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastMessageAt { get; set; }

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; } = null!;

        [ForeignKey(nameof(ReceptionistId))]
        public Receptionist? Receptionist { get; set; }

        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}
