using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_food_delivery_system.Models;
using Online_food_delivery_system.Service;

namespace Online_food_delivery_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize(Roles = "admin,customer")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{email}")]
        [Authorize(Roles = "customer,admin")]
        public async Task<IActionResult> GetCustomerById(string email)
        {
            var customer = await _customerService.GetCustomerByIdAsync(email);
            if (customer == null)
                return NotFound("Customer not found");
            return Ok(customer);
        }
        [HttpPatch("{email}")]
        [Authorize(Roles = "admin, customer")]
        public async Task<IActionResult> UpdatePhoneAddr(string email, [FromBody] UpdatePhoneAddrDTO upd)
        {
            var existing = await _customerService.GetCustomerByIdAsync(email);
            if (existing == null)
                return NotFound("Customer not found");
            existing.Phone = upd.Phone;
            existing.Address = upd.Address;
            await _customerService.UpdateCustomerAsync(existing);
            return NoContent();
        }

        [HttpPost]
        //[Authorize(Roles = "customer,admin")]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {

            await _customerService.AddCustomerAsync(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerID }, customer);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "customer,admin")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            if (id != customer.CustomerID)
                return BadRequest("Customer ID mismatch");

            await _customerService.UpdateCustomerAsync(customer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "customer,admin")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}
