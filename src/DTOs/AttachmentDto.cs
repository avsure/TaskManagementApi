namespace TaskManagementApi.DTOs
{
    public class AttachmentDto
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; } // return public path for download
    }
}
