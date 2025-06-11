using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

[Route("create-checkout-session")]
[ApiController]
public class CheckoutController : ControllerBase
{

  [HttpPost]
  public ActionResult Create()
  {
      var domain = "http://localhost:3000";
      var options = new SessionCreateOptions
      {
          LineItems = new List<SessionLineItemOptions>
          {
            new SessionLineItemOptions
            {
              Price = "price_1RYjUpR3uSJtGqh1U5Aki5N7",
              Quantity = 1,
            },
          },
          Mode = "payment",
          SuccessUrl = domain + "dashboard?success=true",
          CancelUrl = domain + "dashboard?canceled=true",
      };
      var service = new SessionService();
      Session session = service.Create(options);

      return Ok(new { url = session.Url });
  }
}