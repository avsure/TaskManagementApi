using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentsController :ControllerBase
    {
        private readonly IAttachmentService _services;

        public AttachmentsController(IAttachmentService service) => _services = service;


        [HttpPost("{taskId}/attachments")]
        [Authorize(Roles = "Admin,User,Manager")]
        public async Task<IActionResult> UploadAttachment(int taskId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var result = await _services.UploadAttachmentAsync(taskId, file);
            return Ok(result);
        }

        [HttpGet("{taskId}/attachments")]
        [Authorize(Roles = "Admin,User,Manager")]
        public async Task<IActionResult> GetAttachments([FromRoute]int taskId)
        {
            var attachments = await _services.GetAttachmentsByTaskIdAsync(taskId);
            return Ok(attachments);
        }

        [HttpGet("{attachmentId}/attachmentsbyid")]
        [Authorize(Roles = "Admin,User,Manager")]
        public async Task<IActionResult> GetAttachment(int attachmentId)
        {
            var attachment = await _services.GetAttachmentByIdAsync(attachmentId);
            if (attachment == null)
                return NotFound();
            return Ok(attachment);
        }
    }
}
