using TaskManagementApi.DTOs;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto?> GetUserAsync(int id);
        Task<UserDto> CreateUserAsync(UserDto user);
        Task<UserDto?> UpdateUserAsync(int id, UserDto user);
        Task<UserDto?> Login(string username, string password);
    }
}
