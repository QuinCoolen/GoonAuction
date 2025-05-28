public class Bid
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public DateTime Time { get; set; }
    public string Comment { get; set; }
    public int AuctionId { get; set; }
    public Auction Auction { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}