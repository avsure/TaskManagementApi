using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task<User?> Login(string username, string password);
    }
}
