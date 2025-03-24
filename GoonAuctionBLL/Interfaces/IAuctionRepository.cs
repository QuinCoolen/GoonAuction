using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
  public interface IAuctionRepository
  {
    List<AuctionDto> GetAuctions();
    AuctionDto GetAuction(int id);
    CreateEditAuctionDto CreateAuction(CreateEditAuctionDto auctionDto);
    CreateEditAuctionDto UpdateAuction(int id, CreateEditAuctionDto auctionDto);
    bool DeleteAuction(int id);
  }
}