using System.Collections.Generic;
using LuveBLL.Dto;

namespace LuveBLL.Interfaces
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

