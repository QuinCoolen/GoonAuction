using System.Collections.Generic;
using System.Linq;
using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;

namespace GoonAuctionBLL.Services {
  public class AuctionService {
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;

    public AuctionService(IAuctionRepository auctionRepository, IBidRepository bidRepository)
    {
      _auctionRepository = auctionRepository;
      _bidRepository = bidRepository;
    }

    public List<AuctionDto> GetAuctions() {
      return _auctionRepository.GetAuctions();
    }

    public FullAuctionDto GetAuction(int id) {
      return _auctionRepository.GetAuction(id);
    }

    public List<AuctionDto> GetAuctionsByUserId(string userId)
    {
      List<BidDto> bids = _bidRepository.GetBidsByUserId(userId);
      var auctionIds = new HashSet<int>(bids.Select(b => b.AuctionId));

      List<AuctionDto> auctions = _auctionRepository.GetAuctions();
      return auctions.Where(a => auctionIds.Contains(a.Id)).ToList();
    }

    public AuctionDto CreateAuction(CreateEditAuctionDto createEditAuctionDto)
    {
      return _auctionRepository.CreateAuction(createEditAuctionDto);
    }

    public AuctionDto UpdateAuction(int id, CreateEditAuctionDto createEditAuctionDto) {
      return _auctionRepository.UpdateAuction(id, createEditAuctionDto);
    }

    public bool UpdateCurrentPrice(int id, int currentPrice)
    {
      var auction = _auctionRepository.GetAuction(id);
      if (auction == null)
      {
        return false;
      }
      if (currentPrice <= auction.CurrentPrice)
      {
        return false;
      }
      
      AuctionDto updatedAuction = _auctionRepository.UpdateAuction(id, new CreateEditAuctionDto
      {
        Title = auction.Title,
        Description = auction.Description,
        StartingPrice = auction.StartingPrice,
        CurrentPrice = currentPrice,
        ImageUrl = auction.ImageUrl,
        EndDate = auction.EndDate,
        UserId = auction.User.Id
      });

      if (updatedAuction == null)
      {
        return false;
      }
      
      return true;
    }

    public bool DeleteAuction(int id)
    {
      return _auctionRepository.DeleteAuction(id);
    }
  }
}