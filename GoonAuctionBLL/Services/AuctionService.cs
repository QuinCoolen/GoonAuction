using System.Collections.Generic;
using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;

namespace GoonAuctionBLL.Services {
  public class AuctionService : IAuctionRepository {
    private readonly IAuctionRepository _auctionRepository;

    public AuctionService(IAuctionRepository auctionRepository) {
      _auctionRepository = auctionRepository;
    }

    public List<AuctionDto> GetAuctions() {
      return _auctionRepository.GetAuctions();
    }

    public AuctionDto GetAuction(string id) {
      return _auctionRepository.GetAuction(id);
    }

    public CreateEditAuctionDto CreateAuction(CreateEditAuctionDto auctionDto) {
      return _auctionRepository.CreateAuction(auctionDto);
    }

    public CreateEditAuctionDto UpdateAuction(string id, CreateEditAuctionDto auctionDto) {
      return _auctionRepository.UpdateAuction(id, auctionDto);
    }

    public bool DeleteAuction(string id) {
      return _auctionRepository.DeleteAuction(id);
    }
  }
}