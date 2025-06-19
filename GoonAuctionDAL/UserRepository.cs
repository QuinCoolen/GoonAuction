using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;

namespace GoonAuctionDAL
{
  public class UserRepository : IUserRepository
  {
    private readonly DbContext _context;

    public UserRepository(DbContext context)
    {
      _context = context;
    }

    public List<UserDto> GetUsers()
    {
      return _context.Users.Select(u => new UserDto
      {
        Id = u.Id,
        Username = u.UserName,
        Email = u.Email,
      }).ToList();
    }

    public UserDto GetUser(string id)
    {
      var user = _context.Users.Find(id);
      if (user == null)
      {
        throw new Exception("User not found");
      }

      return new UserDto
      {
        Id = user.Id,
        Username = user.UserName,
        Email = user.Email,
      };
    }

    public UserDto GetUserByEmail(string email)
    {
      var user = _context.Users.FirstOrDefault(u => u.Email == email);

      if (user == null)
      {
        return null;
      }

      return new UserDto
      {
        Id = user.Id,
        Username = user.UserName,
        Email = user.Email,
      };
    }

    public void CreateUser(RegisterUserDto EditUserDto)
    {
      var user = new ApplicationUser
      {
        UserName = EditUserDto.UserName,
        Email = EditUserDto.Email,
        PasswordHash = EditUserDto.Password,
      };

      _context.Users.Add(user);
      _context.SaveChanges();
    }

    public void UpdateUser(string id, EditUserDto EditUserDto)
    {
      var user = _context.Users.Find(id);
      if (user == null)
      {
        throw new Exception("User not found");
      }

      user.UserName = EditUserDto.UserName;
      user.Email = EditUserDto.Email;
      user.PasswordHash = EditUserDto.Password;

      _context.SaveChanges();
    }

    public void DeleteUser(string id)
    {
      var user = _context.Users.Find(id);
      if (user == null)
      {
        throw new Exception("User not found");
      }

      _context.Users.Remove(user);
      _context.SaveChanges();
    }
  }
}

