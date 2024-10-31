using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Services
{
    public class OrderServices : IOrder
    {
        public Task<Order> AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<Order> DeleteOrder(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrder(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetOrders()
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateOrder(int id, Order order)
        {
            throw new NotImplementedException();
        }
    }
}
