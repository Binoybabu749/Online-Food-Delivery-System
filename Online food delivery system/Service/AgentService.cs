using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Service
{
    public class AgentService
    {
        private readonly IAgent _agentRepository;

        public AgentService(IAgent agentRepository)
        {
            _agentRepository = agentRepository;
        }

        public async Task<IEnumerable<Agent>> GetAllAgentsAsync()
        {
            return await _agentRepository.GetAllAsync();
        }

        public async Task<Agent> GetAgentByIdAsync(string email)
        {
            return await _agentRepository.GetByIdAsync(email);
        }

        public async Task AddAgentAsync(Agent agent)
        {
            await _agentRepository.AddAsync(agent);
        }

        public async Task UpdateAgentAsync(Agent agent)
        {
            await _agentRepository.UpdateAsync(agent);
        }

        public async Task DeleteAgentAsync(int agentId)
        {
            await _agentRepository.DeleteAsync(agentId);
        }
    }
}
