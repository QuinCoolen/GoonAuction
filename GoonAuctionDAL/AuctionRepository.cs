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
      }).ToList();

      return auctionDtos;
    }
    public AuctionDto GetAuction(int id)
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
      };
      
      return auctionDto;
    }
    public AuctionDto CreateAuction(CreateEditAuctionDto createEditAuctionDto)
    {
      var auction = new Auction
      {
          Title = createEditAuctionDto.Title,
          Description = createEditAuctionDto.Description,
          Starting_price = createEditAuctionDto.StartingPrice,
          Image_url = createEditAuctionDto.ImageUrl,
          End_date = createEditAuctionDto.EndDate,
      };

      _context.Auctions.Add(auction);

      _context.SaveChanges();

      return new AuctionDto
      {
          Id = auction.Id,
          Title = auction.Title,
          Description = auction.Description,
          StartingPrice = auction.Starting_price,
          ImageUrl = auction.Image_url,
          EndDate = auction.End_date,
      };
    }

    public AuctionDto UpdateAuction(int id, CreateEditAuctionDto createEditAuctionDto)
    {
      var auction = _context.Auctions.Find(id);

      if (auction == null)
      {
          return null;
      }

      auction.Title = createEditAuctionDto.Title;
      auction.Description = createEditAuctionDto.Description;
      auction.Starting_price = createEditAuctionDto.StartingPrice;
      auction.Image_url = createEditAuctionDto.ImageUrl;
      auction.End_date = createEditAuctionDto.EndDate;

      _context.SaveChanges();

      return new AuctionDto
      {
          Id = auction.Id,
          Title = auction.Title,
          Description = auction.Description,
          StartingPrice = auction.Starting_price,
          ImageUrl = auction.Image_url,
          EndDate = auction.End_date,
      };
    }

    public bool DeleteAuction(int id)
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