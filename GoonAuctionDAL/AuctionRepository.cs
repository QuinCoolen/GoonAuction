using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;

namespace GoonAuctionDAL
{
  public class AuctionRepository : IAuctionRepository
  {
    private readonly DbContext _context;
    public AuctionRepository(DbContext context)
    {
      _context = context;
    }
    public List<AuctionDto> GetAuctions() 
    {
      var auctions =  _context.Auctions.ToList();

      List<AuctionDto> auctionDtos = auctions.Select(auction => new AuctionDto
      {
          Id = auction.Id,
          Title = auction.Title,
          Description = auction.Description,
          StartingPrice = auction.Starting_price,
          CurrentPrice = auction.Current_price,
          ImageUrl = auction.Image_url,
          EndDate = auction.End_date,
          UserId = auction.UserId
      }).ToList();

      return auctionDtos;
    }
    public AuctionDto GetAuction(string id)
    {
      var auction =  _context.Auctions.Find(id);

      if (auction == null)
      {
          return null;
      }

      var auctionDto = new AuctionDto
      {
          Id = auction.Id,
          Title = auction.Title,
          Description = auction.Description,
          StartingPrice = auction.Starting_price,
          CurrentPrice = auction.Current_price,
          ImageUrl = auction.Image_url,
          EndDate = auction.End_date,
          UserId = auction.UserId
      };
      
      return auctionDto;
    }
    public AuctionDto CreateAuction(AuctionDto auctionDto)
    {
      var auction = new Auction
      {
          Title = auctionDto.Title,
          Description = auctionDto.Description,
          Starting_price = auctionDto.StartingPrice,
          Current_price = auctionDto.CurrentPrice,
          Image_url = auctionDto.ImageUrl,
          End_date = auctionDto.EndDate,
          UserId = auctionDto.UserId
      };

      _context.Auctions.Add(auction);

      _context.SaveChanges();

      return new AuctionDto
      {
          Id = auction.Id,
          Title = auction.Title,
          Description = auction.Description,
          StartingPrice = auction.Starting_price,
          CurrentPrice = auction.Current_price,
          ImageUrl = auction.Image_url,
          EndDate = auction.End_date,
          UserId = auction.UserId
      };
    }

    public AuctionDto UpdateAuction(string id, AuctionDto auctionDto)
    {
      var auction = _context.Auctions.Find(id);

      if (auction == null)
      {
          return null;
      }

      auction.Title = auctionDto.Title;
      auction.Description = auctionDto.Description;
      auction.Starting_price = auctionDto.StartingPrice;
      auction.Current_price = auctionDto.CurrentPrice;
      auction.Image_url = auctionDto.ImageUrl;
      auction.End_date = auctionDto.EndDate;
      auction.UserId = auctionDto.UserId;

      _context.SaveChanges();

      return new AuctionDto
      {
          Id = auction.Id,
          Title = auction.Title,
          Description = auction.Description,
          StartingPrice = auction.Starting_price,
          CurrentPrice = auction.Current_price,
          ImageUrl = auction.Image_url,
          EndDate = auction.End_date,
          UserId = auction.UserId
      };
    }

    public bool DeleteAuction(string id)
    {
      var auction = _context.Auctions.Find(id);

      if (auction == null)
      {
          return false;
      }

      _context.Auctions.Remove(auction);

      _context.SaveChanges();

      return true;
    }
  }
}