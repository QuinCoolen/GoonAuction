namespace GoonAuctionBLL.Services
{
    using System.Collections.Generic;
    using GoonAuctionBLL.Dto;
    using GoonAuctionBLL.Interfaces;

    public class BidService
    {
        private readonly IBidRepository _bidRepository;

        public BidService(IBidRepository bidRepository)
        {
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
            return _bidRepository.PlaceBid(bidDto);
        }
    }
}