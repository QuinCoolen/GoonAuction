using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
    public interface IAuctionService
    {
        List<AuctionDto> GetAuctions();
        FullAuctionDto GetAuction(int id);
        List<AuctionDto> GetAuctionsByUserId(string userId);
        AuctionDto CreateAuction(CreateEditAuctionDto createEditAuctionDto);
        AuctionDto UpdateAuction(int id, CreateEditAuctionDto createEditAuctionDto);
        bool UpdateCurrentPrice(int id, int currentPrice);
        bool UpdateAuctionStatus(int id, AuctionStatusDto status);
        bool DeleteAuction(int id);
    }
} 