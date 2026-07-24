using StajBackendProject.Models;
using StajBackendProject.Models.Dto;

namespace StajBackendProject.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
        TokenResponseDto RefreshToken(RefreshTokenRequestDto request);
    }
}
