using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public interface ITaskRepository
    {
        IQueryable<TaskItem> GetTasks();
        Task<TaskItem?> GetTaskById(int id);
        Task<TaskItem> AddTask(TaskItem task);
        Task<TaskItem> UpdateTask(TaskItem task);
        Task<TaskItem> UpdateTaskPartially(TaskItem task);
        Task<TaskItem?> DeleteTask(int id);
    }
}
