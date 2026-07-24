using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StajBackendProject.Factories;
using StajBackendProject.Interfaces;
using StajBackendProject.Models;
using StajBackendProject.Models.Dto;

namespace StajBackendProject.Implements
{
    public class UserService : IUserService
    {
        private readonly UsersContext _context;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public UserService(UsersContext context, IPasswordHasher hasher, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _hasher = hasher;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public List<UserResponseDto> GetAllUsers()
        {
            var users = _context.Users
                .Include(u => u.Roles)
                .Where(u => u.IsActive == true && u.DeletedAt == null)
                .ToList();

            return _mapper.Map<List<UserResponseDto>>(users);
        }
        public User? GetUserById(int id)
        {
            return _context.Users
                .Include(u => u.Roles)
                .SingleOrDefault(u => u.Id == id && u.IsActive == true);
        }
        public async Task AddNewUserAsync(AddNewUserDto dto)
        {
            string normalizedEmail = dto.Email.Trim().ToLowerInvariant();

            bool isUserExist = await _context.Users.AnyAsync(u => u.Email == normalizedEmail
            || (!string.IsNullOrWhiteSpace(dto.PhoneNumber) && u.PhoneNumber == dto.PhoneNumber));

            if (isUserExist)
            {
                throw new InvalidOperationException("This email or phone number is already registered.");
            }

            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");

            var newUser = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = normalizedEmail,
                IsActive = true,
                InsertDate = DateTime.UtcNow,
                PasswordHash = _hasher.Hash(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                Roles = new List<Role>()
            };

            if (defaultRole != null)
            {
                newUser.Roles.Add(defaultRole);
            }

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            NotificationFactory notificationFactory = new NotificationFactory();

            INotificationService notifier = notificationFactory.CreateNotification("email");

            notifier.Send(dto.Email, $"Welcome {dto.FirstName}! Registration successfully completed.");
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
                user.DeletedAt = DateTime.UtcNow;
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
                user.DeletedAt = null;
                _context.SaveChanges();
            }
            return true;
        }
        public LoginResultDto Login(string email, string password)
        {
            email = email.Trim().ToLowerInvariant();

            var user = _context.Users
                .Include(u => u.Roles)
                .SingleOrDefault(u => u.Email == email && u.IsActive == true);

            if (user == null || !user.IsActive)
            {
                return new LoginResultDto
                {
                    Success = false,
                    Message = "Email or password is incorrect."
                };
            }

            if (user.LockoutEnd.HasValue && user.LockoutEnd <= DateTime.UtcNow)
            {
                user.LockoutEnd = null;
                user.FailedLoginAttempts = 0;
                _context.SaveChanges();
            }

            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                return new LoginResultDto
                {
                    Success = false,
                    Message = "Account is temporarily locked.",
                    LockoutEnd = user.LockoutEnd
                };
            }

            if (!_hasher.Verify(password, user.PasswordHash))
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= 5)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                    user.FailedLoginAttempts = 0;
                }

                _context.SaveChanges();

                return new LoginResultDto
                {
                    Success = false,
                    Message = user.LockoutEnd.HasValue
                        ? "Account has been locked for 15 minutes."
                        : "Email or password is incorrect.",
                    LockoutEnd = user.LockoutEnd
                };
            }

            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            user.LastLoginDate = DateTime.UtcNow;

            string refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            _context.SaveChanges();

            string token = _tokenService.GenerateJwtToken(user);

            return new LoginResultDto
            {
                Success = true,
                Message = "Login successful.",
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }
        public List<User> GetAllUsersOrderByDate() 
        {
            return _context.Users.Where(u => u.IsActive && u.DeletedAt == null).OrderByDescending(u => u.InsertDate).ToList();
        }
        public bool AssignRoleToUser(int userId, int roleId)
        {
            var user = _context.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id == userId);
            if (user == null) throw new Exception("User not found.");

            var role = _context.Roles.Find(roleId);
            if (role == null) throw new Exception("Role not found.");

            if (user.Roles.Any(r => r.Id == roleId))
            {
                throw new Exception("User already has this role.");
            }

            user.Roles.Add(role);
            _context.SaveChanges();

            return true;
        }

        public bool RemoveRoleFromUser(int userId, int roleId)
        {
            var user = _context.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id == userId);
            if (user == null) throw new Exception("User not found.");

            var role = user.Roles.FirstOrDefault(r => r.Id == roleId);
            if (role == null) throw new Exception("The user does not have this role.");

            user.Roles.Remove(role);
            _context.SaveChanges();

            return true;
        }
        public bool ChangePassword(int userId, ChangePasswordDto request)
        {
            var user = _context.Users.Find(userId);
            if (user == null) throw new Exception("User not found.");

            bool isOldPasswordCorrect = _hasher.Verify(request.OldPassword, user.PasswordHash);

            if (!isOldPasswordCorrect)
            {
                throw new Exception("The old password you entered is incorrect.");
            }

            user.PasswordHash = _hasher.Hash(request.NewPassword);

            user.UpdatedAt = DateTime.UtcNow;
            user.PasswordLastChangedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return true;
        }
    }
}
