using System.Security.Claims;
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
    private readonly UserService _userService;

    public BidHub(BidService bidService, UserService userService)
    {
      _bidService = bidService;
      _userService = userService;
    }

    public async Task JoinBid(int auctionId)
    {
      Claim? userId = Context.User.FindFirst(ClaimTypes.NameIdentifier);

      if (userId == null)
      {
        throw new HubException("User not authenticated.");
      }

      var user = _userService.GetUser(userId.Value);

      if (user == null)
      {
        throw new HubException("User not found.");
      }

      await Groups.AddToGroupAsync(Context.ConnectionId, $"Auction_{auctionId}");
      await Clients.Group($"Auction_{auctionId}").SendAsync("UserJoined", user.Username);
    }

    public async Task LeaveBid(int auctionId)
    {
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Auction_{auctionId}");
      await Clients.Group($"Auction_{auctionId}").SendAsync("UserLeft", Context.User.Identity?.Name);
    }

    public async Task PlaceBid(string userId, int auctionId, int amount)
    {
      if (string.IsNullOrEmpty(userId) || auctionId <= 0 || amount <= 0)
      {
        throw new HubException("Invalid bid parameters.");
      }

      UserDto? user = _userService.GetUser(userId); 

      if (user == null)
      {
        throw new HubException("User not found.");
      }

      var bidDto = new BidDto
      {
        UserId = userId,
        AuctionId = auctionId,
        Amount = amount,
        Time = DateTime.UtcNow,
        User = user
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