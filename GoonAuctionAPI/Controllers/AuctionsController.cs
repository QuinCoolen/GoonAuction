using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GoonAuctionAPI.Models;
using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;
using GoonAuctionBLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace GoonAuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public AuctionsController(IAuctionService auctionService)
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
                Increment = auction.Increment,
                Status = auction.Status.ToString(),
                ImageUrl = auction.ImageUrl,
                EndDate = auction.EndDate,
                User = new UserViewModel
                {
                    Id = auction.User.Id,
                    Username = auction.User.Username,
                    Email = auction.User.Email,
                }
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
                Increment = auction.Increment,
                Status = auction.Status,
                ImageUrl = auction.ImageUrl,
                EndDate = auction.EndDate,
                User = new UserViewModel
                {
                    Id = auction.User.Id,
                    Username = auction.User.Username,
                    Email = auction.User.Email,
                },
                Bids = auction.Bids.Select(bid => new BidViewModel
                {
                    Id = bid.Id,
                    Amount = bid.Amount,
                    UserId = bid.UserId,
                    AuctionId = bid.AuctionId,
                    Time = bid.Time,
                    User = new UserViewModel
                    {
                        Id = bid.User.Id,
                        Username = bid.User.Username,
                        Email = bid.User.Email,
                    }
                }).ToList()
            };

            return auctionViewModel;
        }

        [Authorize]
        [HttpGet("user")]
        public List<AuctionViewModel> GetAuctionByUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return null;
            }

            List<AuctionDto> auctions = _auctionService.GetAuctionsByUserId(userId);

            List<AuctionViewModel> auctionViewModels = auctions.Select(auction => new AuctionViewModel
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                StartingPrice = auction.StartingPrice,
                CurrentPrice = auction.CurrentPrice,
                Increment = auction.Increment,
                Status = auction.Status.ToString(),
                ImageUrl = auction.ImageUrl,
                User = new UserViewModel
                {
                    Id = auction.User.Id,
                    Username = auction.User.Username,
                    Email = auction.User.Email,
                },
                EndDate = auction.EndDate,
            }).ToList();

            return auctionViewModels;
        }

        [HttpPatch("{id}/status")]
        public IActionResult UpdateAuctionStatus(int id, string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Status cannot be null or empty.");
            }

            var auction = _auctionService.GetAuction(id);
            if (auction == null)
            {
                return NotFound();
            }
            
            AuctionStatusDto auctionStatus;

            switch (status.ToLower())
            {
                case "notfinished":
                    auctionStatus = AuctionStatusDto.NotFinished;
                    break;
                case "unpaid":
                    auctionStatus = AuctionStatusDto.Unpaid;
                    break;
                case "paymentpending":
                    auctionStatus = AuctionStatusDto.PaymentPending;
                    break;
                case "paid":
                    auctionStatus = AuctionStatusDto.Paid;
                    break;
                default:
                    auctionStatus = auction.Status;
                    break;
            }


            if (!_auctionService.UpdateAuctionStatus(id, auctionStatus))
            {
                return BadRequest("Failed to update auction status.");
            }

            return NoContent();
        }

        // PUT: api/Auctions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutAuction(int id, CreateEditAuctionDto auctionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _auctionService.UpdateAuction(id, auctionDto);
            return NoContent();
        }

        // POST: api/Auctions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostAuction(CreateEditAuctionDto auctionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (auctionDto == null)
            {
                return BadRequest("Auction data cannot be null.");
            }

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
