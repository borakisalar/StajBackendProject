using StajBackendProject.Models;
using StajBackendProject.Models.Dto;

namespace StajBackendProject.Interfaces
{
    public interface IUserService
    {
        List<UserResponseDto> GetAllUsers();
        User? GetUserById(int id);
        Task AddNewUserAsync(AddNewUserDto request);
        bool DeleteUser(int id);
        bool SoftDeleteUserById(int id);
        bool ActivateUser (int id);
        LoginResultDto Login(string email, string password);
        List<User> GetAllUsersOrderByDate();
        bool AssignRoleToUser(int userId, int roleId);
        bool RemoveRoleFromUser(int userId, int roleId);
        bool ChangePassword(int userId, ChangePasswordDto request);
    }
}
