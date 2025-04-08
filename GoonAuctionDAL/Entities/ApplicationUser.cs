using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiry { get; set; }
  public ICollection<Auction> Auctions { get; set; }
  public ICollection<Bid> Bids { get; set; }
}