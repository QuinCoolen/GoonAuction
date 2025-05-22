using System;

namespace GoonAuctionBLL.Dto
{
  public class EditUserDto
  {
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
  }
}

