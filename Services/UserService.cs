using AutoMapper;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Services
{
    public class UserService :  IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        public UserService(IUserRepository repo,IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _repo.GetAllAsync();   // get actual data
            return _mapper.Map<IEnumerable<UserDto>>(users); // map to DTO
        }

        public async Task<UserDto?> GetUserAsync(int id)
        {
           var user =  await _repo.GetByIdAsync(id);
            return _mapper.Map<UserDto?>(user);
        }

        public async Task<UserDto> CreateUserAsync(UserDto user)
        {
            var userEntity = _mapper.Map<User>(user); // map to entity
            var createdUser =  await _repo.AddAsync(userEntity);
            return _mapper.Map<UserDto>(createdUser); // map back to DTO
        }
    }
}
