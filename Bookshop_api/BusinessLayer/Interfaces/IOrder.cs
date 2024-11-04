using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Interfaces
{
    public interface IOrder
    {
        IEnumerable<Order> GetOrders();
        Task<Order> GetOrder(int id);
        String AddOrder(Order order);
        Task<string> UpdateOrder(int id, Order order);
        Task<string> DeleteOrder(int id);
    }
}
