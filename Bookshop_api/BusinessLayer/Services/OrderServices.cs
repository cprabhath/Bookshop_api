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
                    .Include(order => order.OrderItems)
                    .ThenInclude(oi => oi.Book)
                    .ToListAsync();

                if (!orders.Any())
                {
                    return new List<OrderDto>();
                }

                var orderDtos = orders.Select(order => new OrderDto
                {
                    OrderId = order.Id,
                    Id = $"ORD-{DateTime.Now.Year}-{order.Id:D3}",
                    Date = order.CreateAt,
                    Total = order.TotalPrice,
                    Status = order.Status,
                    TrackingNumber = "TRK123456789",
                    EstimatedDelivery = order.CreateAt.AddDays(10),
                    Items = order.OrderItems.Select(item => new OrderItemDto
                    {
                        Id = item.BookId.ToString(),
                        Title = item.Book?.Title,
                        Price = item.Book?.Price ?? 0,
                        Quantity = item.Quantity,
                        Image = item.Book?.Image ?? "default_image_url"
                    }).ToList()
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

        public string GetOrderCountbyCustomerId(int id)
        {
            try
            {
                var result = _context.Orders
                    .Where(o => o.Status != "Cancelled")
                    .Where(o => o.CustomerId == id);

                return result.Count().ToString();

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

        public IEnumerable<Order> GetOrders()
        {
            try
            {
                var result = _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
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
                var result = await _context.Orders
                    .Include(o => o.OrderItems) 
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (result != null)
                {
                    // Update the main order properties
                    result.CustomerId = order.CustomerId;
                    result.Status = order.Status;

                    // Update the order items (BookId, Quantity, Price)
                    foreach (var updatedItem in order.OrderItems)
                    {
                        var existingItem = result.OrderItems
                            .FirstOrDefault(item => item.BookId == updatedItem.BookId);

                        if (existingItem != null)
                        {
                            existingItem.Quantity = updatedItem.Quantity;
                            existingItem.TotalPrice = updatedItem.Quantity * existingItem.Book.Price;
                        }
                        else
                        {
                            // Add new order item if it doesn't exist (optional, depending on your logic)
                            result.OrderItems.Add(new OrderItem
                            {
                                BookId = updatedItem.BookId,
                                Quantity = updatedItem.Quantity,
                                TotalPrice = updatedItem.Quantity * updatedItem.Book.Price
                            });
                        }
                    }

                    // Recalculate the total price for the entire order
                    result.TotalPrice = result.OrderItems.Sum(item => item.TotalPrice);

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
                return ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateOrderStatus(int id, string status)
        {
            try
            {
                var order = _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefault(o => o.Id == id);
                if (order != null)
                {
                    if (status == "Cancelled" && order.Status != "Cancelled")
                    {
                        foreach (var item in order.OrderItems)
                        {
                            var book = _context.Books.FirstOrDefault(b => b.Id == item.BookId);
                            if (book != null)
                            {
                                book.qty += item.Quantity;
                            }
                            else
                            {
                                throw new Exception($"Book with ID {item.BookId} not found for restocking.");
                            }
                        }
                    }
                    order.Status = status;
                    _context.SaveChanges();
                    return "OK";
                }
                else
                {
                    throw new Exception("Order not found");
                }
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
