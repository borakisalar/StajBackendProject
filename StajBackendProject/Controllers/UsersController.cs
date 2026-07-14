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

        // Get : api/Users/Bora
        [HttpGet("name/{FirstName}")]
        public IActionResult GetUserByFirstName(string FirstName)
        {
            var userList = _userService.GetUserByFirstName(FirstName);
            if (userList == null || !userList.Any())
            {
                return NotFound("There is no user with first name " + FirstName + ".");
            }
            return Ok(userList);
        }

        // Get : api/Users/bora@example.com
        [HttpGet("email/{Email}")]
        public IActionResult GetUserByEmail(string Email)
        {
            var user = _userService.GetUserByEmail(Email);
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

        // Delete : api/Users/5
        [HttpDelete("{id}")]
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
        public IActionResult DeactivateUser(int id)
        {
            bool isDeactivated = _userService.DeactivateUser(id);
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
    }
}
