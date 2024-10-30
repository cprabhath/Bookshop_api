using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Data;
using Bookshop_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookshop_api.BusinessLayer.Services
{
    public class CustomerServices : ICustomer
    {
        private readonly ApplicationDBContext _context;
        public CustomerServices(ApplicationDBContext context)
        {
            _context = context;
        }
        public string AddCustomer(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return "OK";
            }
            catch(DbUpdateException ex)
            {
                return ex.InnerException!.Message;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DeleteCustomer(int id)
        {
            try
            {
                var result = await _context.Customers.FindAsync(id);
                if (result != null)
                {
                    result.DeletedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return "OK";
                }
                else
                {
                    return "Customer not found";
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

        public IEnumerable<Customer> GetAllCustomers()
        {
            try
            {
                var result = _context.Customers
                    .Where(b => b.DeletedAt == null)
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

        public async Task<Customer> GetCustomerById(int id)
        {
            try
            {
                var result = await _context.Customers.FindAsync(id);
                return result!;
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

        public async Task<string> UpdateCustomer(int id, Customer customer)
        {
            try
            {
                var result = await _context.Customers.FindAsync(id);
                if (result != null)
                {
                    result.Name = customer.Name;
                    result.Email = customer.Email;
                    result.MobileNumber = customer.MobileNumber;
                    result.Address = customer.Address;
                    result.UpdateAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return "OK";
                }
                else
                {
                    return "Customer not found";
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
