namespace TaskManagementApi.DTOs
{
    //DTOs are API Contracts
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
    }
}
