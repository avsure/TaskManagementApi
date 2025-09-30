using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementApi.DTOs;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _services;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService services, ILogger<TasksController> logger)
        {
            _services = services;
            _logger = logger;
        }

        //GET /api/v1/tasks?status=Open&assignedTo=123&page=1&pageSize=10&sort=dueDate - individual params
        //GET /api/v1/tasks? page = 2 & pageSize = 5
        //GET /api/v1/tasks?status=Open&assignedToUserId=3
        //GET /api/v1/tasks?sortBy=priority&sortDescending=true
        //GET /api/v1/tasks?status=InProgress&assignedToUserId=2&sortBy=title&page=1&pageSize=20
        //caching example:Response cache: GET /api/v1/tasks (cache for 60 seconds) 
        [HttpGet]
        [Authorize(Roles = "Admin,User,Manager")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetTasks([FromQuery] TaskQueryParameters parameters)
        {
            _logger.LogInformation("Fetching tasks with parameters: {@Parameters}", parameters);
            var result = await _services.GetTasksAsync(parameters);

            
            return Ok(new
            {
                result.Data,
                result.TotalCount,
                result.Page,
                result.PageSize
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User,Manager")]
        //Response Header: Example of adding custom headers to the response
        //client side caching: Cache-Control: public, max-age=60(Browser/Postmam/CDN/Proxies)
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _services.GetTaskById(id);
            if (task == null) return NotFound();

            #region Example of adding custom headers to the response

            Response.Headers["Cache-Control"] = "public,max-age=60";
            Response.Headers["X-Task-Version"] = "1.0";

            #endregion

            #region ETAG - Entity Tag

            // Generate a simple ETag (could use rowversion, updated timestamp, or hash)
            // With this null-safe version:
            var updatedAtTicks = task.UpdatedAt.HasValue ? task.UpdatedAt.Value.Ticks : 0;
            var eTag = $"\"task-{task.TaskId}-{updatedAtTicks}\"";

            // Check if client already has this version
            var ifNoneMatch = Request.Headers["If-None-Match"].ToString();
            if (ifNoneMatch == eTag)
            {
                return StatusCode(StatusCodes.Status304NotModified);
            }

            Response.Headers["ETag"] = eTag;

            #endregion

            // With this null-safe version:

            #region Last-Modified example

            // Replace this block in GetTaskById method:

            if (task.UpdatedAt.HasValue)
            {
                var lastModified = task.UpdatedAt.Value.ToUniversalTime().ToString("R");
                //Add If-Modified-Since and its value in request header
                var ifModifiedSince = Request.Headers["If-Modified-Since"].ToString();

                if (!string.IsNullOrEmpty(ifModifiedSince) &&
                    DateTime.TryParse(ifModifiedSince, out var sinceTime) &&
                    task.UpdatedAt <= sinceTime)
                {
                    return StatusCode(StatusCodes.Status304NotModified);
                }

                _logger.LogInformation("Task last modified at: {LastModified}", lastModified);
                Response.Headers["Last-Modified"] = lastModified;
            }

            #endregion

            return Ok(task);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateTask(TaskDto task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get logged-in UserId from JWT
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdValue))
            {
                return Unauthorized("User identifier not found in token.");
            }
            var userId = int.Parse(userIdValue);

            var created = await _services.AddTask(task,userId);

            _logger.LogInformation("Task created with ID: {TaskId} by User ID: {UserId}", created.TaskId, userId);

            return CreatedAtAction(nameof(GetTaskById), new { id = created.TaskId }, created);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTask(TaskDto task)
        {
            var updated = await _services.UpdateTask(task);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateTaskPartially(TaskDto task)
        {
            var updated = await _services.UpdateTaskPartially(task);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _services.DeleteTask(id);
            return NoContent();
        }
    }
}
