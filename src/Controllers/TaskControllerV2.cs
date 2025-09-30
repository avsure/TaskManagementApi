using Microsoft.AspNetCore.Mvc;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        //api/v2/Task
        public IActionResult GetV2()
        {
            return Ok(new { Message = "This is v2 of Tasks API with new format" });
        }
    }
}
