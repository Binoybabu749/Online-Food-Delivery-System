using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Service
{
    public class CustomerService
    {
        private readonly ICustomer _customerRepository;

        public CustomerService(ICustomer customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(string email)
        {
            return await _customerRepository.GetByIdAsync(email);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _customerRepository.AddAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            await _customerRepository.DeleteAsync(customerId);
        }
    }
}
