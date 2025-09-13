using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Data.TaskDbContext _context;
        public UserRepository(Data.TaskDbContext context) => _context = context;

        public async Task<IEnumerable<User>> GetAllAsync() => _context.Users.ToList();
        public async Task<User?> GetByIdAsync(int id) => await _context.Users.FindAsync(id);
        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
