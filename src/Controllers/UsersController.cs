using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService service,ILogger<UsersController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Admin,Manager,User")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _service.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,User")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _service.GetUserAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("create")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            var created = await _service.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = created.UserId }, created);
        }
    }
}
