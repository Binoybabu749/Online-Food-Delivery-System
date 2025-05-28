using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Interface
{
    public interface IAgent
    {
        Task<IEnumerable<Agent>> GetAllAsync();
        Task<Agent> GetByIdAsync(string email);
        Task AddAsync(Agent agent);
        Task UpdateAsync(Agent agent);
        Task DeleteAsync(int agentId);
    }
}
