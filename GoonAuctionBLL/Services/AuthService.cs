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

          var jwtKey = _configuration["Jwt:Key"] ?? "FallbackDevJwtKey_ChangeMe_32Bytes!!"; // 32+ chars (>=256 bits)
          if (Encoding.UTF8.GetByteCount(jwtKey) * 8 < 256)
          {
              throw new InvalidOperationException("JWT signing key length insufficient (<256 bits). Configure Jwt:Key with at least 32 bytes.");
          }
          var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
          var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
          var expirationSetting = _configuration["Jwt:ExpirationHours"];
          double expirationHours = 24; // sane default
          if (!string.IsNullOrWhiteSpace(expirationSetting) && double.TryParse(expirationSetting, out var parsed))
          {
              expirationHours = parsed;
          }
          var expires = DateTime.UtcNow.AddHours(expirationHours);

          var token = new JwtSecurityToken(
              _configuration["Jwt:Issuer"] ?? "http://localhost", // fallback issuer
              _configuration["Jwt:Audience"] ?? "http://localhost", // fallback audience
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
