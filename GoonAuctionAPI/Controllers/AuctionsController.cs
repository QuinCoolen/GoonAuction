using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoonAuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IAuctionRepository _auctionRepository;

        public AuctionsController(DbContext context, IAuctionRepository auctionRepository)
        {
            _context = context;
            _auctionRepository = auctionRepository;
        }

        // GET: api/Auctions
        [HttpGet]
        public List<AuctionViewModel> GetAuctions()
        {
            List<AuctionDto> auctions = _auctionRepository.GetAuctions();

            List<AuctionViewModel> auctionViewModels = auctions.Select(auction => new AuctionViewModel
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                StartingPrice = auction.StartingPrice,
                CurrentPrice = auction.CurrentPrice,
                ImageUrl = auction.ImageUrl,
                EndDate = auction.EndDate,
                UserId = auction.UserId
            }).ToList();

            return auctionViewModels;
        }

        // GET: api/Auctions/5
        [HttpGet("{id}")]
        public AuctionViewModel GetAuction(string id)
        {
            AuctionDto auction = _auctionRepository.GetAuction(id);

            if (auction == null)
            {
                return null;
            }

            var auctionViewModel = new AuctionViewModel
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                StartingPrice = auction.StartingPrice,
                CurrentPrice = auction.CurrentPrice,
                ImageUrl = auction.ImageUrl,
                EndDate = auction.EndDate,
                UserId = auction.UserId
            };

            return auctionViewModel;
        }

        // PUT: api/Auctions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public AuctionViewModel PutAuction(string id, AuctionDto auctionDto)
        {
            AuctionDto updatedAuction = _auctionRepository.UpdateAuction(id, auctionDto);

            if (updatedAuction == null)
            {
                return null;
            }

            var auctionViewModel = new AuctionViewModel
            {
                Id = updatedAuction.Id,
                Title = updatedAuction.Title,
                Description = updatedAuction.Description,
                StartingPrice = updatedAuction.StartingPrice,
                CurrentPrice = updatedAuction.CurrentPrice,
                ImageUrl = updatedAuction.ImageUrl,
                EndDate = updatedAuction.EndDate,
                UserId = updatedAuction.UserId
            };

            return auctionViewModel;
        }

        // POST: api/Auctions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public AuctionViewModel PostAuction(AuctionDto auctionDto)
        {
            AuctionDto createdAuction = _auctionRepository.CreateAuction(auctionDto);

            var auctionViewModel = new AuctionViewModel
            {
                Id = createdAuction.Id,
                Title = createdAuction.Title,
                Description = createdAuction.Description,
                StartingPrice = createdAuction.StartingPrice,
                CurrentPrice = createdAuction.CurrentPrice,
                ImageUrl = createdAuction.ImageUrl,
                EndDate = createdAuction.EndDate,
                UserId = createdAuction.UserId
            };

            return auctionViewModel;
        }

        // DELETE: api/Auctions/5
        [HttpDelete("{id}")]
        public bool DeleteAuction(string id)
        {
            return _auctionRepository.DeleteAuction(id);
        }
    }
}
