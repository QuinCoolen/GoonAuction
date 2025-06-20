using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;
using GoonAuctionBLL.Services;
using Microsoft.EntityFrameworkCore;

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
      var auctions =  _context.Auctions
        .Include(a => a.User)
        .ToList();

      List<AuctionDto> auctionDtos = auctions.Select(auction => new AuctionDto
      {
          Id = auction.Id,
          Title = auction.Title,
          Description = auction.Description,
          StartingPrice = auction.Starting_price,
          CurrentPrice = auction.Current_price,
          Increment = auction.Increment,
          Status = (AuctionStatusDto)auction.Status,
          ImageUrl = auction.Image_url,
          User = new UserDto
          {
            Id = auction.User.Id,
            Username = auction.User.UserName,
            Email = auction.User.Email,
          },
          EndDate = auction.End_date,
      }).ToList();

      return auctionDtos;
    }
    public FullAuctionDto GetAuction(int id)
    {
      Auction? auction =  _context.Auctions.Include(a => a.User).Include(a => a.Bids).ThenInclude(b => b.User).FirstOrDefault(a => a.Id == id);
      
      if (auction == null)
      {
          return null;
      }

      var auctionDto = new FullAuctionDto
      {
        Id = auction.Id,
        Title = auction.Title,
        Description = auction.Description,
        StartingPrice = auction.Starting_price,
        CurrentPrice = auction.Current_price,
        Increment = auction.Increment,
        Status = (AuctionStatusDto)auction.Status,
        ImageUrl = auction.Image_url,
        EndDate = auction.End_date,
        User = new UserDto
        {
          Id = auction.User.Id,
          Username = auction.User.UserName,
          Email = auction.User.Email,
        },
        Bids = auction.Bids.Select(bid => new BidDto
        {
          Id = bid.Id,
          Amount = bid.Amount,
          UserId = bid.UserId,
          AuctionId = bid.AuctionId,
          Time = bid.Time,
          User = new UserDto
          {
            Id = bid.User.Id,
            Username = bid.User.UserName,
            Email = bid.User.Email,
          }
        }).ToList()
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
        Current_price = createEditAuctionDto.StartingPrice,
        Increment = createEditAuctionDto.Increment,
        Status = AuctionStatus.NotFinished,
        Image_url = createEditAuctionDto.ImageUrl,
        End_date = createEditAuctionDto.EndDate,
        UserId = createEditAuctionDto.UserId,
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
      auction.Current_price = createEditAuctionDto.CurrentPrice;
      auction.Increment = createEditAuctionDto.Increment;
      auction.Status = (AuctionStatus)createEditAuctionDto.Status;
      auction.Image_url = createEditAuctionDto.ImageUrl;
      auction.End_date = createEditAuctionDto.EndDate;

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

    public int UpdateExpiredAuctions()
    {
      var expiredAuctions = _context.Auctions
        .Where(a => a.Status == AuctionStatus.NotFinished && a.End_date <= DateTime.UtcNow)
        .ToList();

      foreach (var auction in expiredAuctions)
      {
        auction.Status = AuctionStatus.Unpaid;
      }

      _context.SaveChanges();

      return expiredAuctions.Count;
    }
  }
}