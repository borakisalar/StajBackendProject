using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajBackendProject.Interfaces;
using StajBackendProject.Models.Dto;

namespace StajBackendProject.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UsersController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        // Get : api/Users
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        // Get : api/Users/5
        [HttpGet("{id}")]
        public IActionResult GetUsersById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        // Post : api/Users
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUserByAdmin([FromBody] AddNewUserDto dto)
        {
            try
            {
                await _userService.AddNewUserAsync(dto);
                return Ok("User successfully added.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch 
            {
                return StatusCode(500, "An error occurred on the server side.");
            }
        }

        // Delete : api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            bool isDeleted = _userService.DeleteUser(id);
            if (!isDeleted)
            {
                return NotFound("No user found with given id.");
            }
            return Ok("User successfully deleted.");
        }

        // Put : api/Users/5/deactivate
        [HttpPut("{id}/deactivate")]
        public IActionResult SoftDeleteUserById(int id)
        {
            bool isDeactivated = _userService.SoftDeleteUserById(id);
            if (!isDeactivated)
            {
                return NotFound("User not found.");
            }
            return Ok("User successfully deactivated.");
        }

        // Put : api/Users/5/activate
        [HttpPut("{id}/activate")]
        public IActionResult ActivateUser(int id)
        {
            bool isActivated = _userService.ActivateUser(id);
            if (!isActivated)
            {
                return NotFound("User not found.");
            }
            return Ok("User successfully activated.");
        }

        // Get : api/Users/recent
        [HttpGet("recent")]
        public IActionResult GetAllUsersOrderByDate()
        {
            var users = _userService.GetAllUsersOrderByDate();
            return Ok(users);
        }

        // Get : api/Users/my-info
        [HttpGet("my-info")]
        [Authorize]
        public IActionResult GetMyTokenInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var userRoles = User.Claims
                                .Where(c => c.Type == ClaimTypes.Role)
                                .Select(c => c.Value)
                                .ToList();
            var expClaim = User.FindFirstValue("exp");
            DateTime tokenExpirationDate = DateTime.MinValue;

            if (expClaim != null && long.TryParse(expClaim, out long expSeconds))
            {
                tokenExpirationDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
            }

            return Ok(new
            {
                Message = "Token successfully decoded!",
                UserId = userId,
                Email = userEmail,
                Roles = userRoles,
                TokenExpiresAt = tokenExpirationDate
            });
        }

        // POST api/users/{id}/roles
        [HttpPost("{id}/roles")]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignRole(int id, [FromBody] AssignRoleDto request)
        {
            try
            {
                _userService.AssignRoleToUser(id, request.RoleId);
                return Ok(new { Message = "Role successfully assigned to the user." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE api/users/{id}/roles/{roleId}
        [HttpDelete("{id}/roles/{roleId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveRole(int id, int roleId)
        {
            try
            {
                _userService.RemoveRoleFromUser(id, roleId);
                return Ok(new { Message = "Role successfully removed from the user." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
