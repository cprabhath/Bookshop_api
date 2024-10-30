using Bookshop_api.BusinessLayer.Interfaces;
using Bookshop_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop_api.Controllers
{
    [Authorize(Policy = "SuperPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _customer;
        public CustomerController(ICustomer customer)
        {
            _customer = customer;
        }

        // ====================== Get All Customers ======================
        [HttpGet]
        public IActionResult Get()
        {
            var result = _customer.GetAllCustomers();
            if (result != null && result.Any())
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "No Customers Found");
            }
        }
        // ===============================================================

        // =========================== Add Customer ======================
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            var result = _customer.AddCustomer(customer);

            if (result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Customer Added Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
        // ==============================================================

        // ====================== Update Customer By Id =================
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Customer customer)
        {
            var result = await _customer.UpdateCustomer(id, customer);

            if (result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Customer Updated Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
        // ==============================================================

        // ====================== Delete Customer By Id =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customer.DeleteCustomer(id);

            if (result == "OK")
            {
                return StatusCode(StatusCodes.Status200OK, "Customer Deleted Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, result);
            }
        }
        // ===============================================================

        // ====================== Get Customer By Id =====================
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _customer.GetCustomerById(id);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Customer not found");
            }
        }
        // ===============================================================
    }
}
