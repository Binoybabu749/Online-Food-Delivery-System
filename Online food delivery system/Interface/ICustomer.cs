using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Interface
{
    public interface ICustomer
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(string Email);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(int customerId);
    }
}
