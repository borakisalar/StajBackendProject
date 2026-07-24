using StajBackendProject.Models.Dto;

namespace StajBackendProject.Interfaces
{
    public interface IRoleService
    {
        List<RoleResponseDto> GetAllRoles();
    }
}