using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
  public interface IUserRepository
  {
    List<UserDto> GetUsers();
    UserDto GetUser(string id);
    UserDto GetUserByEmail(string email);
    UserDto CreateUser(CreateEditUserDto createEditUserDto);
    UserDto UpdateUser(string id, CreateEditUserDto createEditUserDto);
    bool DeleteUser(string id);
  }
}

