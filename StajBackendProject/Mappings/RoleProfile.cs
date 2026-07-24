using AutoMapper;
using StajBackendProject.Models;
using StajBackendProject.Models.Dto;

namespace StajBackendProject.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile() 
        {
            CreateMap<Role, RoleResponseDto>();
        }
    }
}
