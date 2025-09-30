using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public interface IAttachmentRepository
    {
        Task AddAsync(Attachment attachment);
        Task<Attachment?> GetByIdAsync(int attachmentId);
        Task<List<Attachment>> GetByTaskIdAsync(int taskId);
    }
}
