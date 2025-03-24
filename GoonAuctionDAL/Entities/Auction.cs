using System.ComponentModel.DataAnnotations;

public class Auction
{
  [Key]
  public string Id { get; set; }
  [Required]
  public string Title { get; set; }
  public string Description { get; set; }
  public int Starting_price { get; set; }
  public int Current_price { get; set; }
  public string Image_url { get; set; }
  public DateTime End_date { get; set; }
  public string UserId { get; set; }  
  public ApplicationUser User { get; set; }
  public ICollection<Bid> Bids { get; set; }
}