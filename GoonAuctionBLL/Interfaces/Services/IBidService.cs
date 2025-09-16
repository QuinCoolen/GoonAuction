using System;
using System.Collections.Generic;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Interfaces
{
    public interface IBidService
    {
        List<BidDto> GetBidsByUserId(string userId);
        BidDto GetBidsByAuctionId(string auctionId);
        bool PlaceBid(BidDto bidDto);
    }
}