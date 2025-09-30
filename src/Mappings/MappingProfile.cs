using TaskManagementApi.DTOs;
using TaskManagementApi.Models;
using AutoMapper;

namespace TaskManagementApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTO
            CreateMap<User, UserDto>();
            CreateMap<TaskItem, TaskDto>();

            // DTO -> Entity
            CreateMap<UserDto, User>();
            CreateMap<TaskDto, TaskItem>();
        }
    }
}
