public class Bid
{
    public string Id { get; set; }
    public int Amount { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string AuctionId { get; set; }
    public Auction Auction { get; set; }
}