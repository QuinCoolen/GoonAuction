using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(UserDto user);
        string GenerateRefreshToken();
    }
} 