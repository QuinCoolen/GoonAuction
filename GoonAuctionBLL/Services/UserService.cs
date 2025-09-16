using System;
using System.Collections.Generic;
using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;

namespace GoonAuctionBLL.Services
{
  public class UserService : IUserService
  {
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public List<UserDto> GetUsers()
    {
      return _userRepository.GetUsers();
    }

    public UserDto GetUser(string id)
    {
      UserDto user = _userRepository.GetUser(id);

      if (user == null)
      {
        return null;
      }
      
      return user;
    }

    public UserDto GetUserByEmail(string email)
    {
      UserDto user = _userRepository.GetUserByEmail(email);

      if (user == null)
      {
        return null;
      }
      
      return user;
    }

    public void CreateUser(RegisterUserDto EditUserDto)
    {
      _userRepository.CreateUser(EditUserDto);
    }

    public void UpdateUser(string id, EditUserDto EditUserDto)
    {
      _userRepository.UpdateUser(id, EditUserDto);
    }

    public void DeleteUser(string id)
    {
      _userRepository.DeleteUser(id);
    }

    public void SaveRefreshToken(string userId, string refreshToken, DateTime expiry)
    {
      UserDto user = _userRepository.GetUser(userId);
      if (user == null)
          throw new ArgumentException("User not found");

      EditUserDto EditUserDto = new EditUserDto
      {
          Email = user.Email,
          UserName = user.Username,
          RefreshToken = refreshToken,
          RefreshTokenExpiryTime = expiry
      };

      _userRepository.UpdateUser(user.Id, EditUserDto);
    }

    public void UpdateRefreshToken(string userId, string refreshToken, DateTime expiry)
    {
      UserDto user = _userRepository.GetUser(userId);
      if (user == null)
          throw new ArgumentException("User not found");

      EditUserDto EditUserDto = new EditUserDto
      {
          Email = user.Email,
          UserName = user.Username,
          RefreshToken = refreshToken,
          RefreshTokenExpiryTime = expiry
      };

      _userRepository.UpdateUser(user.Id, EditUserDto);
    }
  }
}

