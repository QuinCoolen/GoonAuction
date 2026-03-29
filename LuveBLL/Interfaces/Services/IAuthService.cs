using System.Collections.Generic;
using LuveBLL.Dto;

namespace LuveBLL.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(UserDto user);
        string GenerateRefreshToken();
    }
} 