using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace GoonAuctionAPI.Hubs
{
  [Authorize]
  public class BidHub : Hub
  {
    private readonly BidService _bidService;

    public BidHub(BidService bidService)
    {
      _bidService = bidService;
    }

    public async Task PlaceBid(string userId, int auctionId, int amount)
    {
      var bidDto = new BidDto
      {
        UserId = userId,
        AuctionId = auctionId,
        Amount = amount
      };

      bool isBidPlaced = _bidService.PlaceBid(bidDto);

      if (!isBidPlaced)
      {
        throw new HubException("Failed to place bid.");
      }

      await Clients.All.SendAsync("BidPlaced", bidDto);
    }
  }
}