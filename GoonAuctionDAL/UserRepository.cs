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
        UserName = u.UserName,
        Email = u.Email,
      }).ToList();
    }

    public UserDto GetUser(string id)
    {
      var user = _context.Users.Find(id);
      if (user == null)
      {
        return null;
      }

      return new UserDto
      {
        Id = user.Id,
        UserName = user.UserName,
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
        UserName = user.UserName,
        Email = user.Email,
      };
    }

    public UserDto CreateUser(CreateEditUserDto createEditUserDto)
    {
      var user = new ApplicationUser
      {
        UserName = createEditUserDto.UserName,
        Email = createEditUserDto.Email,
        PasswordHash = createEditUserDto.Password,
      };

      _context.Users.Add(user);
      _context.SaveChanges();

      return new UserDto
      {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
      };
    }

    public UserDto UpdateUser(string id, CreateEditUserDto createEditUserDto)
    {
      var user = _context.Users.Find(id);
      if (user == null)
      {
        return null;
      }

      user.UserName = createEditUserDto.UserName;
      user.Email = createEditUserDto.Email;
      user.PasswordHash = createEditUserDto.Password;

      _context.SaveChanges();

      return new UserDto
      {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
      };
    }

    public bool DeleteUser(string id)
    {
      var user = _context.Users.Find(id);
      if (user == null)
      {
        return false;
      }

      _context.Users.Remove(user);
      _context.SaveChanges();

      return true;
    }
  }
}

