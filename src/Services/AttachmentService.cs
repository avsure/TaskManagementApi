using AutoMapper;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IMapper _mapper;
        private readonly TaskDbContext _context;

        public AttachmentService(IAttachmentRepository  attachmentRepository, IMapper mapper, TaskDbContext context)
        {
            _attachmentRepository = attachmentRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<AttachmentDto> UploadAttachmentAsync(int taskId, IFormFile file)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            // Store file in "uploads" folder
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new Attachment
            {
                TaskId = taskId,
                FileName = file.FileName,
                FilePath = filePath,
                FileType = "Image",
                UploadedByUserId = 1,
                UploadedAt = DateTime.Now
            };

            await _attachmentRepository.AddAsync(attachment);

            return new AttachmentDto
            {
                AttachmentId = attachment.AttachmentId,
                FileName = attachment.FileName,
                Url = $"/uploads/{fileName}" // you can later configure Static Files middleware
            };
        }

        public async Task<List<AttachmentDto>> GetAttachmentsByTaskIdAsync(int taskId)
        {
            var attachments = await _attachmentRepository.GetByTaskIdAsync(taskId);
            return attachments.Select(a => new AttachmentDto
            {
                AttachmentId = a.AttachmentId,
                FileName = a.FileName,
                Url = $"/uploads/{Path.GetFileName(a.FilePath)}"
            }).ToList();
        }

        public Task<AttachmentDto> GetAttachmentByIdAsync(int attachmentId)
        {
            var attachment =  _attachmentRepository.GetByIdAsync(attachmentId);

            if (attachment == null)
                throw new KeyNotFoundException("Attachment not found");

            return Task.FromResult(new AttachmentDto 
                {
                AttachmentId = attachment.Result.AttachmentId,
                FileName = attachment.Result.FileName,
                Url = $"/uploads/{Path.GetFileName(attachment.Result.FilePath)}"
            });
        }
    }
}
