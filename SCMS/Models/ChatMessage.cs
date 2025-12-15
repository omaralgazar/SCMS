using System;
using System.ComponentModel.DataAnnotations;

namespace SCMS.Models
{
    
        public enum UserType
        {
            User = 0,
            Patient = 1,
            Staff = 2,
            Doctor = 3,
            Radiologist = 4,
            Receptionist = 5,
            Admin = 6
        }
    

    public class ChatMessage
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public int ThreadId { get; set; }

        [Required]
        public int SenderUserId { get; set; }

        public int? ReceiverUserId { get; set; }

        [Required]
        public UserType SenderType { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        public bool IsRead { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public ChatThread Thread { get; set; } = null!;
        public User SenderUser { get; set; } = null!;
        public User? ReceiverUser { get; set; }
    }
}
