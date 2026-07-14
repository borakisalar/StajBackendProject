using System.Collections;
using StajBackendProject.Models;

namespace StajBackendProject.Interfaces
{
    public interface IUserService
    {
        List<Users> GetAllUsers();
        Users? GetUserById(int id);
        List<Users> GetUserByFirstName(string FirstName);
        Users? GetUserByEmail(string Email);
        void AddNewUser(Users user);
        bool DeleteUser(int id);
        bool DeactivateUser(int id);
        bool ActivateUser (int id);
    }
}
