using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
  public interface IUserRepository
  {
    List<UserDto> GetUsers();
    UserDto GetUser(string id);
    UserDto GetUserByEmail(string email);
    void CreateUser(RegisterUserDto EditUserDto);
    void UpdateUser(string id, EditUserDto EditUserDto);
    void DeleteUser(string id);
  }
}

