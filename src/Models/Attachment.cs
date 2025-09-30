using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models
{
    public class Attachment
    {
        public int AttachmentId { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required, MaxLength(255)]
        public string FileName { get; set; }

        [Required, MaxLength(50)]
        public string FileType { get; set; }

        public long FileSize { get; set; }

        [Required, MaxLength(500)]
        public string FilePath { get; set; } // Local path or cloud storage path

        [Required]
        public int UploadedByUserId { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public TaskItem Task { get; set; }
        public User UploadedBy { get; set; }
    }
}
