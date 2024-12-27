using Bookshop_api.Dto;
using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Interfaces
{
    public interface IOrder
    {
        IEnumerable<Order> GetOrders();
        Task<List<OrderDto>> GetOrder(int id);
        String AddOrder(Order order);
        Task<string> UpdateOrder(int id, Order order);
        Task<string> DeleteOrder(int id);
        String GetOrderCountbyCustomerId(int id);
        String UpdateOrderStatus(int id, string status);
    }
}
