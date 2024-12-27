using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICart _cartService;

        public CartController(ICart cartService)
        {
            _cartService = cartService;
        }

        // ======================= Add to Cart =======================
        [Authorize(Policy = "UserPolicy")]
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(Cart cart)
        {
            try
            {
                var cartItem = await _cartService.AddToCart(cart);
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // ==========================================================

        // ======================= Remove from Cart =================
        [Authorize(Policy = "UserPolicy")]
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromCart(int customerId, int bookId)
        {
            try
            {
                var cartItem = await _cartService.RemoveFromCart(customerId, bookId);
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // ==========================================================

        // ======================= Get Cart by Customer Id ==========
        [Authorize(Policy = "SuperPolicy")]
        [HttpGet("get")]
        public async Task<IActionResult> GetCartByCustomerId(int customerId)
        {
            try
            {
                var cartItems = await _cartService.GetCartByCustomerId(customerId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // ==========================================================

        // ======================= Clear Cart =======================
        [Authorize(Policy = "UserPolicy")]
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart(int customerId)
        {
            try
            {
                await _cartService.ClearCart(customerId);
                return Ok("Cart cleared successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
