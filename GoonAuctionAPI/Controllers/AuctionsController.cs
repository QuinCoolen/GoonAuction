using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public AuctionsController(DbContext context)
        {
            _context = context;
        }

        // GET: api/Auctions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuctions()
        {
            var auctions = await _context.Auctions.ToListAsync();
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
            return Ok(auctionDtos);
        }

        // GET: api/Auctions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuction(string id)
        {
            var auction = await _context.Auctions.FindAsync(id);

            if (auction == null)
            {
                return NotFound();
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

        // PUT: api/Auctions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuction(string id, AuctionDto auctionDto)
        {
            if (id != auctionDto.Id)
            {
                return BadRequest();
            }

            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
            {
                return NotFound();
            }

            // Manually update properties from DTO to Entity
            auction.Title = auctionDto.Title;
            auction.Description = auctionDto.Description;
            auction.Starting_price = auctionDto.StartingPrice;
            auction.Current_price = auctionDto.CurrentPrice;
            auction.Image_url = auctionDto.ImageUrl;
            auction.End_date = auctionDto.EndDate;
            auction.UserId = auctionDto.UserId;


            _context.Entry(auction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuctionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Auctions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuctionDto>> PostAuction(AuctionDto auctionDto)
        {
            var auction = new Auction
            {
                Id = auctionDto.Id,
                Title = auctionDto.Title,
                Description = auctionDto.Description,
                Starting_price = auctionDto.StartingPrice,
                Current_price = auctionDto.CurrentPrice,
                Image_url = auctionDto.ImageUrl,
                End_date = auctionDto.EndDate,
                UserId = auctionDto.UserId
            };

            _context.Auctions.Add(auction);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AuctionExists(auction.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var createdAuctionDto = new AuctionDto
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

            return CreatedAtAction(nameof(GetAuction), new { id = auction.Id }, createdAuctionDto);
        }

        // DELETE: api/Auctions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuction(string id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
            {
                return NotFound();
            }

            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuctionExists(string id)
        {
            return _context.Auctions.Any(e => e.Id == id);
        }
    }
}
