using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

[Route("create-checkout-session")]
[ApiController]
public class CheckoutController : ControllerBase
{
  private readonly AuctionService _auctionService;

  public CheckoutController(AuctionService auctionService)
  {
      _auctionService = auctionService;
  }

  [HttpPost]
  public ActionResult Create(int auctionId)
  {
    FullAuctionDto auction = _auctionService.GetAuction(auctionId);

    if (auction == null)
    {
      return NotFound(new { message = "Auction not found" });
    }

    bool isPending = _auctionService.UpdateAuctionStatus(auctionId, AuctionStatusDto.PaymentPending);

    if (!isPending)
    {
      return BadRequest(new { message = "Failed to update auction status to PaymentPending." });
    }

    var productOptions = new ProductCreateOptions
    {
      Name = auction.Title,
      Description = auction.Description,
    };

    var productService = new ProductService();
    Product product = productService.Create(productOptions);

    if (product == null)
    {
      return BadRequest("Failed to create product.");
    }

    var priceOptions = new PriceCreateOptions
    {
      Product = product.Id,
      Currency = "EUR",
      UnitAmount = auction.CurrentPrice * 100,
    };

    var priceService = new PriceService();
    Price price = priceService.Create(priceOptions);
      
    var domain = "http://localhost:3000/";
    var options = new SessionCreateOptions
    {
        LineItems = new List<SessionLineItemOptions>
        {
          new SessionLineItemOptions
          {
            Price = price.Id,
            Quantity = 1,
          },
        },
        Mode = "payment",
        SuccessUrl = domain + "dashboard?success=true",
        CancelUrl = domain + "dashboard?canceled=true",
    };
    var service = new SessionService();
    Session session = service.Create(options);

    bool isPaid = _auctionService.UpdateAuctionStatus(auctionId, AuctionStatusDto.Paid);

    if (!isPaid)
    {
      return BadRequest(new { message = "Failed to update auction status to Paid." });
    }

    return Ok(new { url = session.Url });
  }
}