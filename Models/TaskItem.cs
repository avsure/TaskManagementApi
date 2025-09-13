using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models
{
    public class TaskItem
    {
        public int TaskId { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        public int? AssignedToUserId { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Open"; // "Open", "In Progress", "Completed", "Cancelled"

        [Required, MaxLength(10)]
        public string Priority { get; set; } = "Medium"; // "Low", "Medium", "High"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public User CreatedBy { get; set; }
        public User AssignedTo { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
    }
}
