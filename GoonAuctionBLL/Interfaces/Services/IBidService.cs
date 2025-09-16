using System;
using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
    public interface IBidService
    {
        List<BidDto> GetBidsByUserId(string userId);
        List<BidDto> GetBidsByAuctionId(int auctionId);
        bool PlaceBid(BidDto bidDto);
    }
}