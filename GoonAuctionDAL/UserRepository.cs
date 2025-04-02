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
        throw new Exception("User not found");
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
        throw new Exception("User not found");
      }

      return new UserDto
      {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
      };
    }

    public void CreateUser(CreateEditUserDto createEditUserDto)
    {
      var user = new ApplicationUser
      {
        UserName = createEditUserDto.UserName,
        Email = createEditUserDto.Email,
        PasswordHash = createEditUserDto.Password,
      };

      _context.Users.Add(user);
      _context.SaveChanges();
    }

    public void UpdateUser(string id, CreateEditUserDto createEditUserDto)
    {
      var user = _context.Users.Find(id);
      if (user == null)
      {
        throw new Exception("User not found");
      }

      user.UserName = createEditUserDto.UserName;
      user.Email = createEditUserDto.Email;
      user.PasswordHash = createEditUserDto.Password;

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

