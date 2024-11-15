using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _orderService;

        public OrderController(IOrder orderService)
        {
            _orderService = orderService;
        }

        // ======================= Get All Orders =======================
        [HttpGet]
        public IActionResult Get()
        {
            var result = _orderService.GetOrders();
            if (result != null && result.Any())
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "No Orders Found");
            }
        }
        // ===========================================================

        // =========================== Add Order ======================
        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            var result = _orderService.AddOrder(order);

            if (result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Order Added Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
        // ============================================================

        // ====================== Get Order By Id ======================
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _orderService.GetOrder(id);

            if (result.Result != null)
            {
                return Ok(result.Result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Order Not Found");
            }
        }
        // ============================================================

        // ====================== Update Order By Id ======================
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Order order)
        {
            var result = await _orderService.UpdateOrder(id, order);

            if (result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Order Updated Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }

        // ======================= Get Orders Count ========================
        [HttpGet("count/{id}")]
        public IActionResult GetOrderCount(int id)
        {
            var result = _orderService.GetOrderCountbyCustomerId(id);

            return Ok(result);
        }

        // =========================== Update Status =====================
        [HttpPut("status/{id}")]
        public IActionResult UpdateOrderStatus(int id, [FromBody] string status)
        {
            var result = _orderService.UpdateOrderStatus(id, status);

            if (result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Order Status Updated Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
    }
}
