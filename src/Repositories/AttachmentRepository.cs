using Microsoft.EntityFrameworkCore;
using System;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly TaskDbContext _context;

        public AttachmentRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Attachment attachment)
        {
            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();
        }

        public async Task<Attachment?> GetByIdAsync(int attachmentId)
        {
            var attachment = await _context.Attachments.FindAsync(attachmentId);
            return attachment;
        }

        public async Task<List<Attachment>> GetByTaskIdAsync(int taskId)
        {
            var attachment = await _context.Attachments
                .Where(a => a.TaskId == taskId)
                .ToListAsync();
            return attachment;
        }
    }
}
