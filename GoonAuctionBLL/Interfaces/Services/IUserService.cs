using System;
using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
    public interface IUserService
    {
        List<UserDto> GetUsers();
        UserDto GetUser(string id);
        UserDto GetUserByEmail(string email);
        void CreateUser(RegisterUserDto EditUserDto);
        void UpdateUser(string id, EditUserDto EditUserDto);
        void DeleteUser(string id);
        void SaveRefreshToken(string userId, string refreshToken, DateTime expiry);
        void UpdateRefreshToken(string userId, string refreshToken, DateTime expiry);
    }
}