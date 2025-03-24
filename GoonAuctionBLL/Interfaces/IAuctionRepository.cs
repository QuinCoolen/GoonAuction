using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
  public interface IAuctionRepository
  {
    List<AuctionDto> GetAuctions();
    AuctionDto GetAuction(string id);
    CreateEditAuctionDto CreateAuction(CreateEditAuctionDto auctionDto);
    CreateEditAuctionDto UpdateAuction(string id, CreateEditAuctionDto auctionDto);
    bool DeleteAuction(string id);
  }
}