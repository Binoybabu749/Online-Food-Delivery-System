using Microsoft.EntityFrameworkCore;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Repository
{
    public class RestaurantRepository : IRestaurant
    {
        private readonly FoodDbContext _context;

        public RestaurantRepository(FoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await _context.Restaurants
                .Include(r => r.MenuItems)
                .Include(r => r.Orders)
                .ToListAsync();
        }

        public async Task<Restaurant> GetByIdAsync(string Email)
        {
            return await _context.Restaurants
                .Include(r => r.MenuItems)
                .Include(r => r.Orders)
                .FirstOrDefaultAsync(r => r.Email == Email);
        }

        public async Task AddAsync(Restaurant restaurant)
        {
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int restaurantId)
        {
            var restaurant = await _context.Restaurants.FindAsync(restaurantId);
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
            }
        }
    }
}