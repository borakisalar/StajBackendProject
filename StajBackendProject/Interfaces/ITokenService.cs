using StajBackendProject.Models;

namespace StajBackendProject.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
