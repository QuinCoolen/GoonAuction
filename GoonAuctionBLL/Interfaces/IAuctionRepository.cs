using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
  public interface IAuctionRepository
  {
    List<AuctionDto> GetAuctions();
    AuctionDto GetAuction(string id);
    AuctionDto CreateAuction(AuctionDto auctionDto);
    AuctionDto UpdateAuction(string id, AuctionDto auctionDto);
    bool DeleteAuction(string id);
  }
}