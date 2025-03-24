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
    public CreateEditAuctionDto CreateAuction(CreateEditAuctionDto auctionDto)
    {
      var auction = new Auction
      {
          Title = auctionDto.Title,
          Description = auctionDto.Description,
          Starting_price = auctionDto.StartingPrice,
          Image_url = auctionDto.ImageUrl,
          End_date = auctionDto.EndDate,
      };

      _context.Auctions.Add(auction);

      _context.SaveChanges();

      return new CreateEditAuctionDto
      {
          Title = auction.Title,
          Description = auction.Description,
          StartingPrice = auction.Starting_price,
          ImageUrl = auction.Image_url,
          EndDate = auction.End_date,
      };
    }

    public CreateEditAuctionDto UpdateAuction(string id, CreateEditAuctionDto auctionDto)
    {
      var auction = _context.Auctions.Find(id);

      if (auction == null)
      {
          return null;
      }

      auction.Title = auctionDto.Title;
      auction.Description = auctionDto.Description;
      auction.Starting_price = auctionDto.StartingPrice;
      auction.Image_url = auctionDto.ImageUrl;
      auction.End_date = auctionDto.EndDate;

      _context.SaveChanges();

      return new CreateEditAuctionDto
      {
          Title = auction.Title,
          Description = auction.Description,
          StartingPrice = auction.Starting_price,
          ImageUrl = auction.Image_url,
          EndDate = auction.End_date,
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