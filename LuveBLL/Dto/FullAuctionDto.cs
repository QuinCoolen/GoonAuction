using System;
using System.Collections.Generic;

namespace LuveBLL.Dto {
  public class FullAuctionDto : AuctionDto
  {
    public List<BidDto> Bids { get; set; } = new List<BidDto>();
    
  }
}