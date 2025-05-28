using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_food_delivery_system.Models;
using Online_food_delivery_system.Service;

namespace Online_food_delivery_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly AgentService _agentService;

        public AgentController(AgentService agentService)
        {
            _agentService = agentService;
        }

        [HttpGet]
        [Authorize(Roles = "admin,agent")]
        public async Task<IActionResult> GetAllAgents()
        {
            var agents = await _agentService.GetAllAgentsAsync();
            return Ok(agents);
        }
        [HttpGet("{email}")]
        [Authorize(Roles = "customer,admin,agent")]
        public async Task<IActionResult>GetAgentById(string email)
        {
            var agent = await _agentService.GetAgentByIdAsync(email);
            if (agent == null)
                return NotFound("Agent not found");
            return Ok(agent);

        }
        [HttpPost]
        [Authorize(Roles = "agent,admin")]
        public async Task<IActionResult> AddAgent([FromBody] AgentDTO agentDTO)
        {
            var agent = new Agent
            {
                Name = agentDTO.Name,
                AgentContact = agentDTO.AgentContact,
                Email = agentDTO.Email,
                IsAvailable = agentDTO.IsAvailable
            }; 

            await _agentService.AddAgentAsync(agent);
            return CreatedAtAction(nameof(GetAllAgents), new { id = agent.AgentID }, agent);
        }
        [HttpPatch("{email}")]
        [Authorize(Roles = "admin, agent")]
        public async Task<IActionResult> UpdatePhoneAddr(string email, [FromBody] AgentcontactDTO upd)
        {
            var existing = await _agentService.GetAgentByIdAsync(email);
            if (existing == null)
                return NotFound("Customer not found");
            existing.AgentContact = upd.Phone;
            //existing.Address = upd.Address;
            await _agentService.UpdateAgentAsync(existing);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAgent(int id, [FromBody] Agent agent)
        {
            if (id != agent.AgentID)
                return BadRequest("Agent ID mismatch");

            await _agentService.UpdateAgentAsync(agent);
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "agent,admin")]
        public async Task<IActionResult> DeleteAgent(int id)
        {
            await _agentService.DeleteAgentAsync(id);
            return NoContent();
        }
    }
}
