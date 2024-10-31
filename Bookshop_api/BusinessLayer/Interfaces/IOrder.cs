using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Interfaces
{
    public interface IOrder
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrder(int id);
        Task<Order> AddOrder(Order order);
        Task<Order> UpdateOrder(int id, Order order);
        Task<Order> DeleteOrder(int id);
    }
}
