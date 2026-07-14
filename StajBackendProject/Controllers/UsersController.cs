using Microsoft.AspNetCore.Mvc;
using StajBackendProject.Models;
using StajBackendProject.Interfaces;

namespace StajBackendProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // Get : api/Users
        [HttpGet]
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
        public IActionResult AddNewUser([FromBody] Users user)
        {
            _userService.AddNewUser(user);
            return Ok("User successfully added.");
        }
    }
}
