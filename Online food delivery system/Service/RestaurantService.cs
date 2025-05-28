using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Service
{
    public class RestaurantService
    {
        private readonly IRestaurant _restaurantRepository;

        public RestaurantService(IRestaurant restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _restaurantRepository.GetAllAsync();
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(string email)
        {
            return await _restaurantRepository.GetByIdAsync(email);
        }

        public async Task AddRestaurantAsync(Restaurant restaurant)
        {
            await _restaurantRepository.AddAsync(restaurant);
        }

        public async Task UpdateRestaurantAsync(Restaurant restaurant)
        {
            await _restaurantRepository.UpdateAsync(restaurant);
        }

        public async Task DeleteRestaurantAsync(int restaurantId)
        {
            await _restaurantRepository.DeleteAsync(restaurantId);
        }
    }
}