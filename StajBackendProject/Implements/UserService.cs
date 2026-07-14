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
        public List<Users> GetUserByFirstName(string FirstName)
        {
            return _context.Users.Where(u => u.FirstName.Contains(FirstName)).ToList();
        }
        public Users? GetUserByEmail(string Email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == Email);
        }
        public void AddNewUser(Users user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public bool DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
        public bool DeactivateUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return false;
            }
            if (user.IsActive)
            {
                user.IsActive = false;
                _context.SaveChanges();
            }
            return true;
        }
        public bool ActivateUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return false;
            }
            if (!user.IsActive)
            {
                user.IsActive = true;
                _context.SaveChanges();
            }
            return true;
        }
    }
}
