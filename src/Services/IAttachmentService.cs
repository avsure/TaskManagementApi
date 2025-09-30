using TaskManagementApi.DTOs;

namespace TaskManagementApi.Services
{
    public interface IAttachmentService
    {
        public Task<AttachmentDto> UploadAttachmentAsync(int taskId, IFormFile file);
        public Task<AttachmentDto> GetAttachmentByIdAsync(int attachmentId);
        public Task<List<AttachmentDto>> GetAttachmentsByTaskIdAsync(int taskId);
    }
}
