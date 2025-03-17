using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
  public ICollection<Auction> Auctions { get; set; }
  public ICollection<Bid> Bids { get; set; }
}