using System.Collections;
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
        void AddNewUser(AddNewUserDto request);
        bool DeleteUser(int id);
        bool SoftDeleteUserById(int id);
        bool ActivateUser (int id);
        bool Login(string Email, string PasswordHash);
    }
}
