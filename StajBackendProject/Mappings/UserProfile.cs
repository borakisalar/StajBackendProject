using AutoMapper;
using StajBackendProject.Models;
using StajBackendProject.Models.Dto;

namespace StajBackendProject.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name)));
        }
    }
}
