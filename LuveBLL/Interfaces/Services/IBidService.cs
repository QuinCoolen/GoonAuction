using System;
using System.Collections.Generic;
using LuveBLL.Dto;

namespace LuveBLL.Interfaces
{
    public interface IBidService
    {
        List<BidDto> GetBidsByUserId(string userId);
        List<BidDto> GetBidsByAuctionId(int auctionId);
        bool PlaceBid(BidDto bidDto);
    }
}