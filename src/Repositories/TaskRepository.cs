using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context) => _context = context;

        public async Task<TaskItem> AddTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public IQueryable<TaskItem> GetTasks()
        {
            // Return as IQueryable so service can filter/sort/paginate
            return _context.Tasks.AsQueryable();
        }

        public async Task<TaskItem?> GetTaskById(int id)
        {
            TaskItem? task = await _context.Tasks.FindAsync(id);
            return task;
        }

        public async Task<TaskItem> UpdateTask(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }
      

        public async Task<TaskItem> UpdateTaskPartially(TaskItem task)
        {
            var existingTask = await _context.Tasks.FindAsync(task.TaskId);

            if (existingTask == null)
            {
                return null; // or throw NotFoundException
            }

            // Update properties
            existingTask.Title = task.Title;
            existingTask.DueDate = task.DueDate;
            existingTask.Priority = task.Priority;
            existingTask.Status = task.Status;

            await _context.SaveChangesAsync();

            return existingTask;
        }

        public async Task<TaskItem?> DeleteTask(int id)
        {
            TaskItem? task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
              _context.Tasks.Remove(task);
              await _context.SaveChangesAsync();
            }
            return task;
        }
    }
}
