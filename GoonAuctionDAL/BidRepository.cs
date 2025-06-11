using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoonAuctionDAL
{
    public class BidRepository : IBidRepository
    {
        private readonly DbContext _context;

        public BidRepository(DbContext context)
        {
            _context = context;
        }

        public List<BidDto> GetBidsByUserId(string userId)
        {
            var bids = _context.Bids
                .Where(b => b.UserId == userId)
                .Include(b => b.User)
                .ToList();

            return bids.Select(bid => new BidDto
            {
                Id = bid.Id,
                Amount = bid.Amount,
                UserId = bid.UserId,
                AuctionId = bid.AuctionId,
            }).ToList();
        }

        public List<BidDto> GetBidsByAuctionId(int auctionId)
        {
            var bids = _context.Bids
                .Where(b => b.AuctionId == auctionId)
                .Include(b => b.User)
                .ToList();

            return bids.Select(bid => new BidDto
            {
                Id = bid.Id,
                Amount = bid.Amount,
                UserId = bid.UserId,
                AuctionId = bid.AuctionId,
            }).ToList();
        }

        public bool PlaceBid(BidDto bidDto)
        {
            var bid = new Bid
            {
                Amount = bidDto.Amount,
                UserId = bidDto.UserId,
                AuctionId = bidDto.AuctionId,
                Time = bidDto.Time.UtcDateTime,
            };

            _context.Bids.Add(bid);
            _context.SaveChanges();
            return true;
        }
    }
}