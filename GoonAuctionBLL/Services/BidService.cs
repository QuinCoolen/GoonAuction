using System;
using System.Collections.Generic;
using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;

namespace GoonAuctionBLL.Services
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly IAuctionService _auctionService;

        public BidService(IBidRepository bidRepository, IAuctionService auctionService)
        {
            _auctionService = auctionService;
            _bidRepository = bidRepository;
        }

        public List<BidDto> GetBidsByUserId(string userId)
        {
            return _bidRepository.GetBidsByUserId(userId);
        }

        public List<BidDto> GetBidsByAuctionId(int auctionId)
        {
            return _bidRepository.GetBidsByAuctionId(auctionId);
        }

        public bool PlaceBid(BidDto bidDto)
        {
            var auction = _auctionService.GetAuction(bidDto.AuctionId);
            if (auction == null || auction.EndDate < DateTime.UtcNow)
            {
                return false;
            }

            if (bidDto.Amount <= auction.CurrentPrice)
            {
                return false;
            }

            _auctionService.UpdateCurrentPrice(bidDto.AuctionId, bidDto.Amount);

            return _bidRepository.PlaceBid(bidDto);
        }

        public BidDto GetBidsByAuctionId(string auctionId)
        {
            throw new NotImplementedException();
        }

        void IBidService.PlaceBid(BidDto bidDto)
        {
            throw new NotImplementedException();
        }
    }
}