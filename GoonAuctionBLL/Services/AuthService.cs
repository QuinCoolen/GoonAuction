using System.Security.Claims;
using GoonAuctionBLL.Dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using GoonAuctionBLL.Interfaces;

namespace GoonAuctionBLL.Services
{
    public class AuthService : IAuthService
    {
      private readonly IConfiguration _configuration;

      public AuthService(IConfiguration configuration)
      {
          _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
      }
        
      public string GenerateJwtToken(UserDto user)
      {
          if (user == null)
              throw new ArgumentNullException(nameof(user));

          var claims = new List<Claim>
          {
              new Claim(ClaimTypes.NameIdentifier, user.Id),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.Name, user.Username),
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

      public string GenerateRefreshToken()
      {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
      }
    }
}
