using System.ComponentModel.DataAnnotations;

namespace GoonAuctionBLL.Dto
{
  public class LoginUserDto
  {
    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
    [MinLength(5, ErrorMessage = "Email must be at least 5 characters long.")]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "Password must be at least 6 characters long and cannot be longer than 100 characters.", MinimumLength = 6)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    [Display(Name = "Password")]
    public string Password { get; set; }
  }
}
