using System.Collections.Generic;
using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;

namespace GoonAuctionBLL.Services {
  public class AuctionService {
    private readonly IAuctionRepository _auctionRepository;

    public AuctionService(IAuctionRepository auctionRepository) {
      _auctionRepository = auctionRepository;
    }

    public List<AuctionDto> GetAuctions() {
      return _auctionRepository.GetAuctions();
    }

    public FullAuctionDto GetAuction(int id) {
      return _auctionRepository.GetAuction(id);
    }

    public AuctionDto CreateAuction(CreateEditAuctionDto createEditAuctionDto) {
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