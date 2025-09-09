namespace GoonAuctionAPI.Models
{
  public class FullAuctionViewModel : AuctionViewModel
  {
    public List<BidViewModel> Bids { get; set; } = new List<BidViewModel>();
  }
}