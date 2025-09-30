namespace TaskManagementApi.DTOs
{
    public class TaskQueryParameters
    {
        public string? Status { get; set; }
        public int? AssignedToUserId { get; set; }
        public string? SortBy { get; set; } = "DueDate"; // default sort
        public bool SortDescending { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
