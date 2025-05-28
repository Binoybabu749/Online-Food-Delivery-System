using Microsoft.EntityFrameworkCore;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Repository
{
    public class CustomerRepository : ICustomer
    {
        private readonly FoodDbContext _context;

        public CustomerRepository(FoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .Include(c => c.Orders) // Include related Orders
                    .ThenInclude(o => o.Restaurant) // Include Restaurant in Orders
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Payment) // Include Payment in Orders
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Delivery) // Include Delivery in Orders
                        .ThenInclude(d => d.Agent) // Include Agent in Delivery
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderMenuItems) // Include OrderMenuItems in Orders
                        .ThenInclude(omi => omi.MenuItem) // Include MenuItem in OrderMenuItems
                .ToListAsync();
        }


        public async Task<Customer> GetByIdAsync(string Email)
        {
            return await _context.Customers
                .Include(c => c.Orders) // Include related Orders
                    .ThenInclude(o => o.Restaurant) // Include Restaurant in Orders
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Payment) // Include Payment in Orders
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Delivery) // Include Delivery in Orders
                        .ThenInclude(d => d.Agent) // Include Agent in Delivery
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderMenuItems) // Include OrderMenuItems in Orders
                        .ThenInclude(omi => omi.MenuItem) // Include MenuItem in OrderMenuItems
                .FirstOrDefaultAsync(c => c.Email == Email);
        }


        public async Task AddAsync(Customer customer)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == customer.Email);
            if(existingUser == null)
            {
                var user = new User
                {
                    Username = customer.Name,
                    Email = customer.Email,
                    Password = "default",
                    Role = "customer"
                };
                
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
