using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
  public interface IAuctionRepository
  {
    List<AuctionDto> GetAuctions();
    FullAuctionDto GetAuction(int id);
    AuctionDto CreateAuction(CreateEditAuctionDto createEditAuctionDto);
    AuctionDto UpdateAuction(int id, CreateEditAuctionDto createEditAuctionDto);
    bool DeleteAuction(int id);
    int UpdateExpiredAuctions();
  }
}