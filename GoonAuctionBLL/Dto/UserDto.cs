using System.ComponentModel.DataAnnotations;

namespace GoonAuctionBLL.Dto
{
  public class UserDto
  {
    public string Id { get; set; }
    [Required(ErrorMessage = "User name is required.")]
    [StringLength(50, ErrorMessage = "User name cannot be longer than 50 characters.")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number.")]
    [Display(Name = "Password")]
    public string Password { get; set; }
  }
}

