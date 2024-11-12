using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Data;
using Bookshop_api.Dto;
using Bookshop_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookshop_api.BusinessLayer.Services
{
    public class OrderServices : IOrder
    {
        private readonly ApplicationDBContext _context;

        public OrderServices(ApplicationDBContext context)
        {
            _context = context;
        }

        public string AddOrder(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return "Ok";
            }
            catch (DbUpdateException ex)
            {
                return ex.InnerException!.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DeleteOrder(int id)
        {
            try
            {
                var result = await _context.Orders.FindAsync(id);
                if (result != null)
                {
                    result.DeletedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return "Ok";
                }
                else
                {
                    return "Order not found";
                }
            }
            catch (DbUpdateException ex)
            {
                return ex.InnerException!.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<OrderDto>> GetOrder(int customerId)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(order => order.CustomerId == customerId)
                    .Include(order => order.Book)
                    .ToListAsync();

                if (!orders.Any())
                {
                    return new List<OrderDto>();
                }

                // Map the orders to the desired structure
                var orderDtos = orders.Select(order => new OrderDto
                {
                    Id = $"ORD-{DateTime.Now.Year}-{order.Id:D3}",
                    Date = order.CreateAt,  
                    Total = order.TotalPrice,
                    Status = order.Status,
                    TrackingNumber = "TRK123456789", 
                    EstimatedDelivery = order.CreateAt.AddDays(5), 
                    Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    Id = order.BookId.ToString(),
                    Title = order.Book?.Title,
                    Price = order.Book?.Price ?? 0,
                    Quantity = order.Quantity,
                    Image = order.Book?.Image ?? "default_image_url" 
                }
            }
                }).ToList();

                return orderDtos;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Order> GetOrders()
        {
            try
            {
                var result = _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Book)
                    .Where(o => o.DeletedAt == null)
                    .ToList();
                return result;
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

        public async Task<string> UpdateOrder(int id, Order order)
        {
            try
            {
                var result = await _context.Orders.FindAsync(id);
                if (result != null)
                {
                    result.CustomerId = order.CustomerId;
                    result.BookId = order.BookId;
                    result.Quantity = order.Quantity;
                    result.TotalPrice = order.TotalPrice;
                    await _context.SaveChangesAsync();
                    return "Ok";
                }
                else
                {
                    return "Order not found";
                }
            }
            catch (DbUpdateException ex)
            {
                return ex.InnerException!.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
