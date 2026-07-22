using StajBackendProject.Models;
using StajBackendProject.Models.Dto;

namespace StajBackendProject.Interfaces
{
    public interface IUserService
    {
        List<Users> GetAllUsers();
        Users? GetUserById(int id);
        List<Users> GetUserByFirstName(string FirstName);
        Users? GetUserByEmail(string Email);
        Task AddNewUserAsync(AddNewUserDto request);
        bool DeleteUser(int id);
        bool SoftDeleteUserById(int id);
        bool ActivateUser (int id);
        LoginResultDto Login(string email, string password);
        List<Users> GetAllUsersOrderByDate();
    }
}
