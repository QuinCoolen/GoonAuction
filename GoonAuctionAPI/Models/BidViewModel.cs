namespace GoonAuctionAPI.Models
{
    public class BidViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AuctionId { get; set; }
        public int Amount { get; set; }
        public DateTimeOffset Time { get; set; }
        public UserViewModel User { get; set; }
    }
}