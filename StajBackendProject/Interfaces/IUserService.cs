using System.Collections;
using StajBackendProject.Models;

namespace StajBackendProject.Interfaces
{
    public interface IUserService
    {
        List<Users> GetAllUsers();
        Users? GetUserById(int id);
        void AddNewUser(Users user);
    }
}
