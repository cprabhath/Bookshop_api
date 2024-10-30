using Bookshop_api.Models;

namespace Bookshop_api.BusinessLayer.Interfaces
{
    public interface ICustomer
    {
        IEnumerable<Customer> GetAllCustomers();
        String AddCustomer(Customer customer);
        Task<string> UpdateCustomer(int id, Customer customer);
        Task<string> DeleteCustomer(int id);
        Task<Customer> GetCustomerById(int id);
    }
}
