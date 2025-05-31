public class CreateEditAuctionViewModel
{
  public string Title { get; set; }
  public string Description { get; set; }
  public int StartingPrice { get; set; }
  public int CurrentPrice { get; set; }
  public int Increment { get; set; }
  public string ImageUrl { get; set; }
  public DateTime EndDate { get; set; }
}