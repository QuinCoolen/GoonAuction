using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Services;
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
        
        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            return Ok(_userService.GetUserByEmail(email));
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] CreateEditUserDto userDto)
        {
            _userService.CreateUser(userDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, [FromBody] CreateEditUserDto userDto)
        {
            _userService.UpdateUser(id, userDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            _userService.DeleteUser(id);
            return Ok();    
        }
    }
}