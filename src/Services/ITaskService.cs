using TaskManagementApi.DTOs;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services
{
    public interface ITaskService
    {
        Task<PagedResult<TaskDto>> GetTasksAsync(TaskQueryParameters parameters);
        Task<TaskDto?> GetTaskById(int id);
        Task<TaskDto> AddTask(TaskDto task,int userId);
        Task<TaskDto> UpdateTask(TaskDto task);
        Task<TaskDto> UpdateTaskPartially(TaskDto task);
        Task<TaskDto> DeleteTask(int id);
        Task<List<TaskDto>?> GetTasksCachedAsync();
    }
}
