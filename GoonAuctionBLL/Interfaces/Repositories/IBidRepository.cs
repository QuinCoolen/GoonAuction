using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
  public interface IBidRepository
  {
    List<BidDto> GetBidsByUserId(string userId);
    List<BidDto> GetBidsByAuctionId(int auctionId);
    bool PlaceBid(BidDto bidDto);
  }
}