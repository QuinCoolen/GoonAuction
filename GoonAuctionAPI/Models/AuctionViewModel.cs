using GoonAuctionBLL.Dto;

namespace GoonAuctionAPI.Models
{
  public class AuctionViewModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StartingPrice { get; set; }
    public int CurrentPrice { get; set; }
    public int Increment { get; set; }
    public AuctionStatusDto Status { get; set; }
    public string ImageUrl { get; set; }
    public UserViewModel User { get; set; }
    public DateTime EndDate { get; set; }
  }
}