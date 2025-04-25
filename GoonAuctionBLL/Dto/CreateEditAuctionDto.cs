using System;

namespace GoonAuctionBLL.Dto {
  public class CreateEditAuctionDto {
    public string Title { get; set; }
    public string Description { get; set; }
    public int StartingPrice { get; set; }
    public string ImageUrl { get; set; }
    public DateTime EndDate { get; set; }
    public string UserId { get; set; }
  }
}