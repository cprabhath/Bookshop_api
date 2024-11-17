using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Models;
using Bookshop_api.Utils;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Stripe_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {

        private readonly IOrder _orderService;
        private readonly IBook _bookService;
        private readonly ICustomer _customerService;

        public PaymentsController(IOrder orderService, IBook bookService, ICustomer customer)
        {
            _orderService = orderService;
            _bookService = bookService;
            _customerService = customer;
        }


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
                            Currency = "lkr",
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
                    SuccessUrl = "http://localhost:5173/#/payment/processing?sessionId={CHECKOUT_SESSION_ID}",
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
        [HttpPost("confirm-order")]
        public async Task<IActionResult> ConfirmOrder([FromBody] ConfirmOrderRequest request)
        {
            try
            {
                var service = new SessionService();
                var session = await service.GetAsync(request.SessionId);

                if (session.PaymentStatus == "paid")
                {
                    var orderNumber = $"ORD-{DateTime.Now.Year}-{Guid.NewGuid().ToString().Substring(0, 3)}";

                    var order = new Order
                    {
                        OrderId = orderNumber,
                        CustomerId = request.CustomerId,
                        CreateAt = DateTime.UtcNow,
                        Status = "Pending",
                        TotalPrice = 0.0,
                        OrderItems = new List<OrderItem>()
                    };

                    var orderItemsForEmail = new List<(string itemName, int quantity, double price)>();

                    foreach (var item in request.CartItems)
                    {
                        var orderItem = new OrderItem
                        {
                            BookId = item.BookId,
                            Quantity = item.Quantity,
                            TotalPrice = item.Price * item.Quantity,
                        };

                        order.TotalPrice += orderItem.TotalPrice;
                        order.OrderItems.Add(orderItem);

                        var book = await _bookService.GetBookById(item.BookId);
                        if (book != null)
                        {
                            if (book.qty < 0)
                            {
                                return BadRequest(new { message = $"Insufficient stock for book {book.Title}." });
                            }

                            await _bookService.UpdateBookQty(book.Id, item.Quantity);
                            orderItemsForEmail.Add((book.Title, item.Quantity, item.Price));
                        }
                        else
                        {
                            return NotFound(new { message = $"Book with ID {item.BookId} not found." });
                        }
                    }
                    _orderService.AddOrder(order);
                    var customer = _customerService.GetCustomerById(request.CustomerId);
                    
                    if(customer != null)
                    {
                        string email = customer.Result.Email;
                        string orderId = orderNumber;
                        string orderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        string customerName = customer.Result.Name;

                        OrderApprovalEmail emailService = new OrderApprovalEmail();
                        string send = emailService.SendOrderApproveMail(email, orderId, orderDate, customerName, orderItemsForEmail);
                        if (send != "OK")
                        {
                            return BadRequest(new { message = send });
                        }
                    }

                    return Ok(new { message = "Payment confirmed.", orderNumber });
                }
                else
                {
                    return BadRequest(new { message = "Payment not confirmed." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }


    public class CheckoutSessionRequest
    {
        public long Amount { get; set; } = 0;
        public string ProductName { get; set; } = string.Empty;
    }

}
