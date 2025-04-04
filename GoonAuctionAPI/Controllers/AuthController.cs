using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoonAuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
      private readonly IConfiguration _configuration;
      private readonly UserService _userService;
      public AuthController(UserService userService, IConfiguration configuration)
      {
          _userService = userService;
          _configuration = configuration;
      }

      [HttpPost("login")]
      public IActionResult Login([FromBody] LoginUserDto model)
      {
          var user = _userService.GetUserByEmail(model.Email);
          if (user == null)
          {
              return Unauthorized();
          }

          var token = GenerateJwtToken(user);
          return Ok(new { token });
      }

      private string GenerateJwtToken(UserDto user)
      {
          var claims = new List<Claim>
          {
              new Claim(ClaimTypes.NameIdentifier, user.Id),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.Name, user.UserName),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
          };

          var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
          var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
          var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpirationHours"]));

          var token = new JwtSecurityToken(
              _configuration["Jwt:Issuer"],
              _configuration["Jwt:Audience"],
              claims,
              expires: expires,
              signingCredentials: creds
          );

          return new JwtSecurityTokenHandler().WriteToken(token);
      }
  }
}