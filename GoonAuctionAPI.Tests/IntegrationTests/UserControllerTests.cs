using System.Net;
using System.Text;
using System.Text.Json;
using GoonAuctionBLL.Dto;
using Xunit;

namespace GoonAuctionAPI.Tests.IntegrationTests
{
    public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        #region GET /api/users Tests

        [Fact]
        public async Task GetUsers_ReturnsListOfUsers()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var response = await client.GetAsync("/api/user");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<object>>(content, _jsonOptions);

            Assert.NotNull(users);
            Assert.True(users.Count > 0);
        }

        [Fact]
        public async Task GetUser_WithValidId_ReturnsUser()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var userId = "test-user-1";
            var response = await client.GetAsync($"/api/user/{userId}");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<object>(content, _jsonOptions);

            Assert.NotNull(user);
        }

        [Fact]
        public async Task GetUser_WithInvalidId_ReturnsOkWithNull()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var response = await client.GetAsync($"/api/user/invalid-id");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("", content); // Since GetUser returns null
        }

        #endregion

        #region GET /api/user/email/{email} Tests

        [Fact]
        public async Task GetUserByEmail_WithValidEmail_ReturnsUser()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var email = "testuser1@example.com";
            var response = await client.GetAsync($"/api/user/email/{email}");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<object>(content, _jsonOptions);

            Assert.NotNull(user);
        }

        [Fact]
        public async Task GetUserByEmail_WithInvalidEmail_ReturnsOkWithNull()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var email = "doesnotexist@example.com";
            var response = await client.GetAsync($"/api/user/email/{email}");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("", content);
        }

        #endregion

        #region POST /api/user/register Tests

        [Fact]
        public async Task RegisterUser_WithValidData_ReturnsOk()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var newUser = new RegisterUserDto
            {
                UserName = "newuser",
                Email = "newuser@example.com",
                Password = "Password123!"
            };

            var json = JsonSerializer.Serialize(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/user/register", content);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("User created successfully", responseContent);
        }

        [Fact]
        public async Task RegisterUser_WithInvalidData_ReturnsBadRequest()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var newUser = new RegisterUserDto
            {
                UserName = "",
                Email = "invalidemail",
                Password = ""
            };

            var json = JsonSerializer.Serialize(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/user/register", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region PUT /api/user/{id} Tests

        [Fact]
        public async Task UpdateUser_WithValidData_ReturnsNoContent()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var userDto = new EditUserDto
            {
                UserName = "updateduser",
                Email = "updated@example.com"
            };

            var json = JsonSerializer.Serialize(userDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/user/test-user-1", content);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion

        #region DELETE /api/user/{id} Tests

        [Fact]
        public async Task DeleteUser_WithValidId_ReturnsNoContent()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var response = await client.DeleteAsync("/api/user/test-user-2");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion
    }
}
