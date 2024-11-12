using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Interfaces
{
    public interface ICart
    {
        Task<Cart> AddToCart(Cart cart);
        Task<Cart> RemoveFromCart(int customerId, int bookId);
        Task<List<Cart>> GetCartByCustomerId(int customerId);
        Task ClearCart(int customerId);
    }
}
