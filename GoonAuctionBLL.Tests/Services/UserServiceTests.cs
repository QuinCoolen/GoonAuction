using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using GoonAuctionBLL.Services;
using GoonAuctionBLL.Interfaces;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public void GetUsers_WhenCalled_ReturnsAllUsers()
        {
            // Arrange
            var expectedUsers = new List<UserDto>
            {
                new UserDto { Id = "1", Email = "test1@example.com", Username = "user1" },
                new UserDto { Id = "2", Email = "test2@example.com", Username = "user2" }
            };
            _mockUserRepository.Setup(repo => repo.GetUsers())
                .Returns(expectedUsers);

            // Act
            var result = _userService.GetUsers();

            // Assert
            Assert.Equal(expectedUsers, result);
            _mockUserRepository.Verify(repo => repo.GetUsers(), Times.Once);
        }

        [Fact]
        public void GetUser_WithValidId_ReturnsUser()
        {
            // Arrange
            var userId = "1";
            var expectedUser = new UserDto { Id = userId, Email = "test@example.com", Username = "testuser" };
            _mockUserRepository.Setup(repo => repo.GetUser(userId))
                .Returns(expectedUser);

            // Act
            var result = _userService.GetUser(userId);

            // Assert
            Assert.Equal(expectedUser, result);
            _mockUserRepository.Verify(repo => repo.GetUser(userId), Times.Once);
        }

        [Fact]
        public void SaveRefreshToken_WithValidUser_UpdatesUser()
        {
            // Arrange
            var userId = "1";
            var refreshToken = "new-refresh-token";
            var expiry = DateTime.UtcNow.AddDays(7);
            var existingUser = new UserDto { Id = userId, Email = "test@example.com", Username = "testuser" };

            _mockUserRepository.Setup(repo => repo.GetUser(userId))
                .Returns(existingUser);

            // Act
            _userService.SaveRefreshToken(userId, refreshToken, expiry);

            // Assert
            _mockUserRepository.Verify(repo => repo.UpdateUser(
                userId,
                It.Is<EditUserDto>(dto => 
                    dto.Email == existingUser.Email &&
                    dto.UserName == existingUser.Username &&
                    dto.RefreshToken == refreshToken &&
                    dto.RefreshTokenExpiryTime == expiry
                )),
                Times.Once
            );
        }

        [Fact]
        public void SaveRefreshToken_WithInvalidUserId_ThrowsArgumentException()
        {
            // Arrange
            var userId = "invalid-id";
            _mockUserRepository.Setup(repo => repo.GetUser(userId))
                .Returns((UserDto)null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                _userService.SaveRefreshToken(userId, "token", DateTime.UtcNow));
        }

        [Fact]
        public void GetUserByEmail_WithValidEmail_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var expectedUser = new UserDto { Id = "1", Email = email, Username = "testuser" };
            _mockUserRepository.Setup(repo => repo.GetUserByEmail(email))
                .Returns(expectedUser);

            // Act
            var result = _userService.GetUserByEmail(email);

            // Assert
            Assert.Equal(expectedUser, result);
            _mockUserRepository.Verify(repo => repo.GetUserByEmail(email), Times.Once);
        }

        [Fact]
        public void GetUserByEmail_WithInvalidEmail_ReturnsNull()
        {
            // Arrange
            var email = "nonexistent@example.com";
            _mockUserRepository.Setup(repo => repo.GetUserByEmail(email))
                .Returns((UserDto)null);

            // Act
            var result = _userService.GetUserByEmail(email);

            // Assert
            Assert.Null(result);
            _mockUserRepository.Verify(repo => repo.GetUserByEmail(email), Times.Once);
        }

        [Fact]
        public void CreateUser_WithValidData_CallsRepository()
        {
            // Arrange
            var newUser = new RegisterUserDto 
            { 
                Email = "new@example.com", 
                UserName = "newuser",
                Password = "password123"
            };

            // Act
            _userService.CreateUser(newUser);

            // Assert
            _mockUserRepository.Verify(repo => repo.CreateUser(newUser), Times.Once);
        }

        [Fact]
        public void UpdateUser_WithValidData_CallsRepository()
        {
            // Arrange
            var userId = "1";
            var updateData = new EditUserDto 
            { 
                Email = "updated@example.com", 
                UserName = "updateduser" 
            };

            // Act
            _userService.UpdateUser(userId, updateData);

            // Assert
            _mockUserRepository.Verify(repo => repo.UpdateUser(userId, updateData), Times.Once);
        }

        [Fact]
        public void DeleteUser_WithValidId_CallsRepository()
        {
            // Arrange
            var userId = "1";

            // Act
            _userService.DeleteUser(userId);

            // Assert
            _mockUserRepository.Verify(repo => repo.DeleteUser(userId), Times.Once);
        }

        [Fact]
        public void UpdateRefreshToken_WithValidUser_UpdatesUser()
        {
            // Arrange
            var userId = "1";
            var refreshToken = "new-refresh-token";
            var expiry = DateTime.UtcNow.AddDays(7);
            var existingUser = new UserDto { Id = userId, Email = "test@example.com", Username = "testuser" };

            _mockUserRepository.Setup(repo => repo.GetUser(userId))
                .Returns(existingUser);

            // Act
            _userService.UpdateRefreshToken(userId, refreshToken, expiry);

            // Assert
            _mockUserRepository.Verify(repo => repo.UpdateUser(
                userId,
                It.Is<EditUserDto>(dto => 
                    dto.Email == existingUser.Email &&
                    dto.UserName == existingUser.Username &&
                    dto.RefreshToken == refreshToken &&
                    dto.RefreshTokenExpiryTime == expiry
                )),
                Times.Once
            );
        }

        [Fact]
        public void UpdateRefreshToken_WithInvalidUserId_ThrowsArgumentException()
        {
            // Arrange
            var userId = "invalid-id";
            _mockUserRepository.Setup(repo => repo.GetUser(userId))
                .Returns((UserDto)null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                _userService.UpdateRefreshToken(userId, "token", DateTime.UtcNow));
        }
    }
} 