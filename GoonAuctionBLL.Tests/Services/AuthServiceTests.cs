using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using GoonAuctionBLL.Services;
using GoonAuctionBLL.Dto;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GoonAuctionBLL.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthService _authService;
        private const string TestJwtKey = "your-256-bit-secret-your-256-bit-secret-your-256-bit-secret-your-256-bit-secret";
        private const string TestIssuer = "test-issuer";
        private const string TestAudience = "test-audience";
        private const string TestExpirationHours = "24";

        public AuthServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(x => x["Jwt:Key"]).Returns(TestJwtKey);
            _mockConfiguration.Setup(x => x["Jwt:Issuer"]).Returns(TestIssuer);
            _mockConfiguration.Setup(x => x["Jwt:Audience"]).Returns(TestAudience);
            _mockConfiguration.Setup(x => x["Jwt:ExpirationHours"]).Returns(TestExpirationHours);

            _authService = new AuthService(_mockConfiguration.Object);
        }

        [Fact]
        public void GenerateJwtToken_WithValidUser_ReturnsValidToken()
        {
            // Arrange
            var user = new UserDto
            {
                Id = "1",
                Email = "test@example.com",
                Username = "testuser"
            };

            // Act
            var token = _authService.GenerateJwtToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            // Verify token contents
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Verify claims
            Assert.Equal(user.Id, jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Assert.Equal(user.Email, jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value);
            Assert.Equal(user.Username, jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.NotNull(jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

            // Verify token properties
            Assert.Equal(TestIssuer, jwtToken.Issuer);
            Assert.Equal(TestAudience, jwtToken.Audiences.First());
            Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
            Assert.True(jwtToken.ValidTo <= DateTime.UtcNow.AddHours(double.Parse(TestExpirationHours)));
        }

        [Fact]
        public void GenerateRefreshToken_ReturnsValidToken()
        {
            // Act
            var token1 = _authService.GenerateRefreshToken();
            var token2 = _authService.GenerateRefreshToken();

            // Assert
            Assert.NotNull(token1);
            Assert.NotNull(token2);
            Assert.NotEmpty(token1);
            Assert.NotEmpty(token2);
            Assert.NotEqual(token1, token2); // Tokens should be unique
            Assert.True(token1.Length >= 32); // Base64 encoded 32 bytes should be at least 32 characters
        }

        [Fact]
        public void GenerateJwtToken_WithNullUser_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _authService.GenerateJwtToken(null));
        }

        [Fact]
        public void GenerateJwtToken_WithMissingConfiguration_ThrowsException()
        {
            // Arrange
            var invalidConfig = new Mock<IConfiguration>();
            var invalidService = new AuthService(invalidConfig.Object);
            var user = new UserDto { Id = "1", Email = "test@example.com", Username = "testuser" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => invalidService.GenerateJwtToken(user));
        }
    }
} 