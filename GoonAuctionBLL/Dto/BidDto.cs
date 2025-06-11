using System;

namespace GoonAuctionBLL.Dto
{
    public class BidDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AuctionId { get; set; }
        public int Amount { get; set; }
        public DateTimeOffset Time { get; set; }
        public UserDto User { get; set; }
    }
}