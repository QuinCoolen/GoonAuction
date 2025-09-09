using System.Net;
using System.Net.Http.Json;
using GoonAuctionBLL.Dto;

namespace GoonAuctionAPI.Tests.IntegrationTests
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        #region POST /api/auth/login Tests

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkAndSetsCookies()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            // Arrange
            var loginDto = new LoginUserDto
            {
                Email = "testuser1@example.com",
                Password = "Password123!"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Login successful", content);

            // Check cookies are set
            Assert.Contains("Set-Cookie", response.Headers.ToString());
            Assert.Contains("jwt=", response.Headers.ToString());
            Assert.Contains("refresh=", response.Headers.ToString());
        }

        [Fact]
        public async Task Login_WithInvalidEmail_ReturnsNotFound()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            // Arrange
            var loginDto = new LoginUserDto
            {
                Email = "nonexistent@test.com",
                Password = "Password123!"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("User not found", content);
        }

        [Fact]
        public async Task Login_WithInvalidModel_ReturnsBadRequest()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            // Arrange: missing email
            var loginDto = new LoginUserDto
            {
                Email = "",
                Password = ""
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region POST /api/auth/logout Tests

        [Fact]
        public async Task Logout_WithoutAuthentication_ReturnsUnauthorized()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/auth/logout", null);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        #endregion
    }
}
