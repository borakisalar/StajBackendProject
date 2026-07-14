using System.Collections;
using StajBackendProject.Interfaces;
using StajBackendProject.Models;

namespace StajBackendProject.Implements
{
    public class UserService : IUserService
    {
        private readonly UsersContext _context;
        public UserService(UsersContext context)
        {
            _context = context;
        }

        public List<Users> GetAllUsers()
        {
            return _context.Users.ToList();
        }
        public Users? GetUserById(int id)
        {
            return _context.Users.Find(id);
        }
        public void AddNewUser(Users user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
