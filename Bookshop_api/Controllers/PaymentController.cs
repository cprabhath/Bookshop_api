using Bookshop_api.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Stripe_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutRequest request)
        {
            try
            {
                var lineItems = new List<SessionLineItemOptions>();

                foreach (var item in request.CartItems)
                {
                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.ProductName
                            },
                            UnitAmount = (long)(item.Amount * 100) 
                        },
                        Quantity = item.Quantity
                    });
                }

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = lineItems,
                    Mode = "payment",
                    SuccessUrl = "http://localhost:5173/#/payment/complete",
                    CancelUrl = "http://localhost:5173/#/payment/cancelled",
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);
                return Ok(new { sessionId = session.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }

    public class CheckoutSessionRequest
    {
        public long Amount { get; set; }
        public string ProductName { get; set; }
    }

}
