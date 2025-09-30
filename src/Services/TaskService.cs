using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public TaskService(ITaskRepository taskRepository, IMapper mapper,IMemoryCache cache)
        {
            _repository = taskRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<TaskDto> AddTask(TaskDto task,int userId)
        {
            TaskItem taskItem = _mapper.Map<TaskItem>(task);
            taskItem.CreatedByUserId = userId;
            TaskItem newTask = await _repository.AddTask(taskItem);
            return _mapper.Map<TaskDto>(newTask);
        }

        public async Task<TaskDto> DeleteTask(int id)
        {
            TaskItem? taskItem = await _repository.DeleteTask(id);
            return _mapper.Map<TaskDto>(taskItem);
        }

        public async Task<TaskDto?> GetTaskById(int id)
        {
            TaskItem? taskItem = await _repository.GetTaskById(id);
            return _mapper.Map<TaskDto>(taskItem);
        }

        public async Task<PagedResult<TaskDto>> GetTasksAsync(TaskQueryParameters parameters)
        {
            var query = _repository.GetTasks(); // IQueryable<TaskItem>

            // filtering
            // Filtering
            if (!string.IsNullOrEmpty(parameters.Status))
                query = query.Where(t => t.Status == parameters.Status);

            if (parameters.AssignedToUserId.HasValue)
                query = query.Where(t => t.AssignedToUserId == parameters.AssignedToUserId);


            // Sorting
            query = parameters.SortBy?.ToLower() switch
            {
                "title" => parameters.SortDescending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
                "priority" => parameters.SortDescending ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority),
                "duedate" => parameters.SortDescending ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate),
                _ => query.OrderBy(t => t.DueDate) // fallback
            };

            // Paging
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<TaskDto>>(items);

            return new PagedResult<TaskDto>
            {
                Data = _mapper.Map<List<TaskDto>>(items),
                TotalCount = totalCount,
                Page = parameters.Page,
                PageSize = parameters.PageSize
            };
        }

        // Caching example - In-memory caching(Server side caching)
        public async Task<List<TaskDto>?> GetTasksCachedAsync()
        {
            if (!_cache.TryGetValue("tasksList", out List<TaskDto>? tasks))
            {
                // Fix: GetTasks() returns IQueryable<TaskItem>, so we need to enumerate it asynchronously
                var query = _repository.GetTasks();
                var taskItems = await query.ToListAsync();
                tasks = _mapper.Map<List<TaskDto>>(taskItems);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60));

                _cache.Set("tasksList", tasks, cacheEntryOptions);
            }

            return tasks;
        }

        public async Task<TaskDto> UpdateTask(TaskDto task)
        {
            TaskItem taskItem = await _repository.UpdateTask(_mapper.Map<TaskItem>(task));
            return _mapper.Map<TaskDto>(taskItem);
        }

        public async Task<TaskDto> UpdateTaskPartially(TaskDto task)
        {
            TaskItem taskItem = await _repository.UpdateTaskPartially(_mapper.Map<TaskItem>(task));
            return _mapper.Map<TaskDto>(taskItem);
        }
    }
}
