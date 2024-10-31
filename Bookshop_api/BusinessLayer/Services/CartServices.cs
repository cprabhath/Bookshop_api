using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Data;
using Bookshop_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookshop_api.BusinessLayer.Services
{
    public class CartServices : ICart
    {
        private readonly ApplicationDBContext _context;

        public CartServices(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Cart> AddToCart(int customerId, int bookId, int quantity)
        {
            try
            {
                var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.CustomerId == customerId && c.BookId == bookId);

                if (cartItem != null)
                {
                    cartItem.Quantity += quantity;
                    cartItem.TotalPrice = cartItem.Quantity * cartItem.Book.Price;
                }
                else
                {
                    cartItem = new Cart
                    {
                        CustomerId = customerId,
                        BookId = bookId,
                        Quantity = quantity,
                        TotalPrice = quantity * (await _context.Books.FindAsync(bookId))?.Price ?? 0.0,
                        Status = "Pending"
                    };
                    _context.Carts.Add(cartItem);
                }

                await _context.SaveChangesAsync();
                return cartItem;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Cart> RemoveFromCart(int customerId, int bookId)
        {
            try
            {
                var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.CustomerId == customerId && c.BookId == bookId);

                if (cartItem == null) throw new Exception("Item not found in cart");

                cartItem.Quantity -= 1;
                if (cartItem.Quantity <= 0)
                {
                    _context.Carts.Remove(cartItem);
                }
                else
                {
                    cartItem.TotalPrice = cartItem.Quantity * cartItem.Book.Price;
                }

                await _context.SaveChangesAsync();
                return cartItem;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Cart>> GetCartByCustomerId(int customerId)
        {
            try
            {
                var cartItems = await _context.Carts
                    .Include(c => c.Book)
                    .Where(c => c.CustomerId == customerId)
                    .ToListAsync();

                return cartItems;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ClearCart(int customerId)
        {
            try
            {
                var cartItems = await _context.Carts.Where(c => c.CustomerId == customerId).ToListAsync();
                if (!cartItems.Any()) throw new Exception("No items in the cart to clear");

                _context.Carts.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
