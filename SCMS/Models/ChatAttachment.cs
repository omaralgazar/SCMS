using System.ComponentModel.DataAnnotations;

namespace SCMS.Models
{
    public class ChatAttachment
    {
        [Key]
        public int AttachmentId { get; set; }

        [Required]
        public int MessageId { get; set; }

        [Required]
        public string FilePath { get; set; } = null!;

        public string? FileName { get; set; }
        public string? ContentType { get; set; }

        public ChatMessage Message { get; set; } = null!;
    }
}
