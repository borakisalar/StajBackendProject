using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajBackendProject.Interfaces;
using StajBackendProject.Models.Dto;

namespace StajBackendProject.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] AddNewUserDto dto)
        {
            try
            {
                await _userService.AddNewUserAsync(dto);
                return Ok(new { Message = "User successfully registered." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { Message = "An error occurred on the server side." });
            }
        }

        // POST : api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDto request)
        {
            var result = _userService.Login(request.Email, request.Password);

            if (result.Success)
            {
                return Ok(result);
            }

            if (result.LockoutEnd.HasValue)
            {
                return StatusCode(StatusCodes.Status423Locked, result);
            }

            return Unauthorized(result);
        }

        // POST : api/auth/refresh-token
        [HttpPost("refresh-token")]
        [AllowAnonymous] // Refresh token endpoint should be accessible when the access token is dead
        public IActionResult RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            try
            {
                var tokens = _tokenService.RefreshToken(request);
                return Ok(tokens);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        // POST : api/auth/change-password
        [HttpPost("change-password")]
        [Authorize] // Only logged-in users can change their password
        public IActionResult ChangePassword([FromBody] ChangePasswordDto request)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized(new { Message = "Invalid or expired token." });
                }

                _userService.ChangePassword(userId, request);

                return Ok(new { Message = "Password changed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}