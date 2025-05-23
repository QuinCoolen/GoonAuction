using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;
using GoonAuctionBLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoonAuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionService _auctionService;

        public AuctionsController(AuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        // GET: api/Auctions
        [HttpGet]
        public List<AuctionViewModel> GetAuctions()
        {
            List<AuctionDto> auctions = _auctionService.GetAuctions();

            List<AuctionViewModel> auctionViewModels = auctions.Select(auction => new AuctionViewModel
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                StartingPrice = auction.StartingPrice,
                CurrentPrice = auction.CurrentPrice,
                ImageUrl = auction.ImageUrl,
                EndDate = auction.EndDate,
            }).ToList();

            return auctionViewModels;
        }

        // GET: api/Auctions/5
        [HttpGet("{id}")]
        public FullAuctionViewModel GetAuction(int id)
        {
            FullAuctionDto auction = _auctionService.GetAuction(id);

            if (auction == null)
            {
                return null;
            }

            var auctionViewModel = new FullAuctionViewModel
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                StartingPrice = auction.StartingPrice,
                CurrentPrice = auction.CurrentPrice,
                ImageUrl = auction.ImageUrl,
                EndDate = auction.EndDate,
                User = new UserViewModel
                {
                    Id = auction.User.Id,
                    UserName = auction.User.UserName,
                    Email = auction.User.Email,
                },
            };

            return auctionViewModel;
        }

        // PUT: api/Auctions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutAuction(int id, CreateEditAuctionDto auctionDto)
        {
            _auctionService.UpdateAuction(id, auctionDto);
            return NoContent();
        }

        // POST: api/Auctions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostAuction(CreateEditAuctionDto auctionDto)
        {
            _auctionService.CreateAuction(auctionDto);
            return NoContent();
        }

        // DELETE: api/Auctions/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAuction(int id)
        {
            _auctionService.DeleteAuction(id);
            return NoContent();
        }
    }
}
