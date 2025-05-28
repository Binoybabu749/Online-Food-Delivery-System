using Microsoft.EntityFrameworkCore;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Repository
{
    public class AgentRepository : IAgent
    {
        private readonly FoodDbContext _context;

        public AgentRepository(FoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Agent>> GetAllAsync()
        {
            return await _context.Agents
                .Include(a => a.Deliveries) // Include related Deliveries
                    .ThenInclude(d => d.Order) // Include related Order in Deliveries
                        .ThenInclude(o => o.Customer) // Include Customer in Order
                .Include(a => a.Deliveries)
                    .ThenInclude(d => d.Order)
                        .ThenInclude(o => o.Restaurant) // Include Restaurant in Order
                .Include(a => a.Deliveries)
                    .ThenInclude(d => d.Order)
                        .ThenInclude(o => o.Payment) // Include Payment in Order
                .Include(a => a.Deliveries)
                    .ThenInclude(d => d.Order)
                        .ThenInclude(o => o.OrderMenuItems) // Include OrderMenuItems in Order
                .ToListAsync();
        }

        public async Task<Agent> GetByIdAsync(string email)
        {
            return await _context.Agents
                .Include(a => a.Deliveries) // Include related Deliveries
                    .ThenInclude(d => d.Order) // Include related Order in Deliveries
                        .ThenInclude(o => o.Customer) // Include Customer in Order
                .Include(a => a.Deliveries)
                    .ThenInclude(d => d.Order)
                        .ThenInclude(o => o.Restaurant) // Include Restaurant in Order
                .Include(a => a.Deliveries)
                    .ThenInclude(d => d.Order)
                        .ThenInclude(o => o.Payment) // Include Payment in Order
                .Include(a => a.Deliveries)
                    .ThenInclude(d => d.Order)
                        .ThenInclude(o => o.OrderMenuItems) // Include OrderMenuItems in Order
                .FirstOrDefaultAsync(a => a.Email== email);
        }

        public async Task AddAsync(Agent agent)
        {
            await _context.Agents.AddAsync(agent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Agent agent)
        {
            _context.Agents.Update(agent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int agentId)
        {
            var agent = await _context.Agents.FindAsync(agentId);
            if (agent != null)
            {
                _context.Agents.Remove(agent);
                await _context.SaveChangesAsync();
            }
        }
    }
}
