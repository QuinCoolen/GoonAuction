public class Bid
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public int AuctionId { get; set; }
    public Auction Auction { get; set; }
}