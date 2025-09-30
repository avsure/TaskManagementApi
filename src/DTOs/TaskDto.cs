using System.ComponentModel.DataAnnotations;
using TaskManagementApi.Helpers;

namespace TaskManagementApi.DTOs
{
    public class TaskDto
    {
        public int TaskId { get; set; }

        [Required(ErrorMessage ="Title is required")]
        [StringLength(100,MinimumLength =5, ErrorMessage ="Title must be between 5 and 100 characters.")]
        public required string Title { get; set; }
        public string? Description { get; set; }

        //dont keep CreatedByUserId property. Its a security gap.
        //That means the real source of truth (the JWT claims) is not being used.

        //public int CreatedByUserId { get; set; }
        public int? AssignedToUserId { get; set; }

        [Required]
        [EnumDataType(typeof(TasksStatus), ErrorMessage = "Invalid status value")]
        public string Status { get; set; }

        [Required]
        [EnumDataType(typeof(TaskPriority))]
        public string Priority { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [FutureDate("CreatedDate", ErrorMessage = "Due date must be after created date")]
        public DateTime? DueDate { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public enum TasksStatus
    {
        Open,
        InProgress,
        Completed,
        Canceller
    }
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }
}
