using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
  public interface IUserRepository
  {
    List<UserDto> GetUsers();
    UserDto GetUser(string id);
    UserDto GetUserByEmail(string email);
    void CreateUser(CreateEditUserDto createEditUserDto);
    void UpdateUser(string id, CreateEditUserDto createEditUserDto);
    void DeleteUser(string id);
  }
}

