using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        [Required, MaxLength(20)]
        public string Role { get; set; }  // "User", "Manager", "Admin"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<TaskItem> CreatedTasks { get; set; }
        public ICollection<TaskItem> AssignedTasks { get; set; }
        public ICollection<Attachment> UploadedAttachments { get; set; }
    }
}
