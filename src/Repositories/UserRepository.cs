using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskDbContext _context;

        public UserRepository(TaskDbContext context) => _context = context;

        public async Task<IEnumerable<User>> GetAllAsync() => _context.Users.ToList();

        public async Task<User?> GetByIdAsync(int id) => await _context.Users.FindAsync(id);

        public async Task<User> AddAsync(User user)
        {
            if (user != null)
            {
                //hardcoded, since creating user is out of scope
                if (String.IsNullOrEmpty(user.HashedPassword)) { user.HashedPassword = "hashedpassword4"; }
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> Login(string username, string password)
        {
            if(String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password)) 
                return await Task.FromResult<User?>(null);
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.HashedPassword == password);
            return await Task.FromResult<User?>(user);
        }
    }
}
