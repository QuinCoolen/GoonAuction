using System.Security.Claims;
using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoonAuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            return Ok(_userService.GetUser(id));
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            // 1. Get the user ID from the JWT claims
            Claim? userId = User.FindFirst(ClaimTypes.NameIdentifier);
            Console.WriteLine("UserId" + userId?.Value);
            if (userId == null)
                return Unauthorized();

            // 2. Fetch the user from the database
            var user = _userService.GetUser(userId.Value);
            if (user == null)
                return NotFound();

            // 3. Return the user info (customize as needed)
            return Ok(new
            {
                id = user.Id,
                username = user.UserName,
                email = user.Email
            });
        }
        
        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            return Ok(_userService.GetUserByEmail(email));
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] CreateEditUserDto userDto)
        {
            _userService.CreateUser(userDto);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, [FromBody] CreateEditUserDto userDto)
        {
            _userService.UpdateUser(id, userDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            _userService.DeleteUser(id);
            return NoContent();    
        }
    }
}