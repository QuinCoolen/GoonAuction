using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace GoonAuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
      private readonly UserService _userService;
      private readonly AuthService _authService;
      public AuthController(UserService userService, AuthService authService)
      {
          _userService = userService;
          _authService = authService;
      }

      [HttpPost("login")]
      public IActionResult Login([FromBody] LoginUserDto model)
      {
        UserDto user = _userService.GetUserByEmail(model.Email);
        if (user == null)
        {
            return Unauthorized();
        }

        var token = _authService.GenerateJwtToken(user);
        var refreshToken = _authService.GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);

        HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = refreshTokenExpiration
        });

        _userService.SaveRefreshToken(user.Id, refreshToken, refreshTokenExpiration);

        HttpContext.Response.Cookies.Append("refresh", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = refreshTokenExpiration
        });

        return Ok(new { Message = "Login successful", Token = token, RefreshToken = refreshToken, RefreshTokenExpiration = refreshTokenExpiration });
      }
  }
}