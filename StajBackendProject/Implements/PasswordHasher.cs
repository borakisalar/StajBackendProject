using StajBackendProject.Interfaces;

namespace StajBackendProject.Implements
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password) 
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11);
        }

        public bool Verify(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
