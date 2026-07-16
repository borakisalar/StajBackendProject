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
            return _context.Users.Where(u => u.IsActive == true).ToList();
        }
        public Users? GetUserById(int id)
        {
            return _context.Users.SingleOrDefault(u => u.Id == id && u.IsActive == true);
        }
        public List<Users> GetUserByFirstName(string FirstName)
        {
            return _context.Users.Where(u => u.FirstName.Contains(FirstName) && u.IsActive == true).ToList();
        }
        public Users? GetUserByEmail(string Email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == Email && u.IsActive == true);
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
        public bool SoftDeleteUserById(int id)
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
