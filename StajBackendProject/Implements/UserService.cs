using System.Collections;
using StajBackendProject.Interfaces;
using StajBackendProject.Models;
using StajBackendProject.Models.Dto;

namespace StajBackendProject.Implements
{
    public class UserService : IUserService
    {
        private readonly UsersContext _context;
        private readonly IPasswordHasher _hasher;
        public UserService(UsersContext context, IPasswordHasher hasher)
        {
            _context = context;
            _hasher = hasher;
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
        public void AddNewUser(AddNewUserDto dto)
        {
            var newUser = new Users
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                IsActive = true,
                InsertDate = DateTime.Now,
                PasswordHash = _hasher.Hash(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                Role = "User"
            };
            _context.Users.Add(newUser);
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
        public bool Login(string email, string password) 
        {
            var user = GetUserByEmail(email);
            if (user == null || !user.IsActive)
            {
                return false;
            }
            if (user.PasswordHash == _hasher.Hash(password)) 
            {
                return true;
            }
            return false;
        }
        public List<Users> GetAllUsersOrderByDate() 
        {
            return _context.Users.Where(u => u.IsActive).OrderByDescending(u => u.InsertDate).ToList();
        }
    }
}
