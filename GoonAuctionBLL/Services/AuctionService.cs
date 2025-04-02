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

    public AuctionDto GetAuction(int id) {
      return _auctionRepository.GetAuction(id);
    }

    public AuctionDto CreateAuction(CreateEditAuctionDto createEditAuctionDto) {
      return _auctionRepository.CreateAuction(createEditAuctionDto);
    }

    public AuctionDto UpdateAuction(int id, CreateEditAuctionDto createEditAuctionDto) {
      return _auctionRepository.UpdateAuction(id, createEditAuctionDto);
    }

    public bool DeleteAuction(int id) {
      return _auctionRepository.DeleteAuction(id);
    }
  }
}