using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Stripe_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        [HttpPost("create-checkout-session")]
        public IActionResult CreateCheckoutSession([FromBody] CheckoutSessionRequest request)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = request.ProductName
                        },
                        UnitAmount = request.Amount
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                SuccessUrl = "http://localhost:5173/#/?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "http://localhost:5173/#/cancelled"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Ok(new { sessionId = session.Id });
        }
    }

    public class CheckoutSessionRequest
    {
        public long Amount { get; set; }
        public string ProductName { get; set; }
    }

}
