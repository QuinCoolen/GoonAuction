using System;
using System.Collections.Generic;
using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;

namespace GoonAuctionBLL.Services
{
  public class UserService : IUserRepository
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
      return _userRepository.GetUser(id);
    }

    public UserDto GetUserByEmail(string email)
    {
      return _userRepository.GetUserByEmail(email);
    }

    public void CreateUser(CreateEditUserDto createEditUserDto)
    {
      _userRepository.CreateUser(createEditUserDto);
    }

    public void UpdateUser(string id, CreateEditUserDto createEditUserDto)
    {
      _userRepository.UpdateUser(id, createEditUserDto);
    }

    public void DeleteUser(string id)
    {
      _userRepository.DeleteUser(id);
    }

     public void UpdateRefreshToken(string userId, string refreshToken, DateTime expiry)
        {
            UserDto user = _userRepository.GetUser(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            CreateEditUserDto createEditUserDTO = new CreateEditUserDto
            {
                Email = user.Email,
                UserName = user.UserName,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = expiry
            };

            _userRepository.UpdateUser(user.Id, createEditUserDTO);
        }
  }
}

