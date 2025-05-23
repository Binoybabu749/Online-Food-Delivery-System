using Microsoft.AspNetCore.Mvc;
using Online_food_delivery_system.Models;
using Online_food_delivery_system.Service;

namespace Online_food_delivery_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly DeliveryService _deliveryService;

        public DeliveryController(DeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _deliveryService.GetAllDeliveriesAsync();
            return Ok(deliveries);
        }
        [HttpPost("assign/auto")]
        public async Task<IActionResult> AssignDeliveryAgentAutomatically(int orderId, DateTime estimatedTimeOfArrival)
        {
            try
            {
                await _deliveryService.AssignDeliveryAgentAutomaticallyAsync(orderId, estimatedTimeOfArrival);
                return Ok("Delivery agent assigned automatically and successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("assign")]
        public async Task<IActionResult> AssignDeliveryAgent(int orderId, int agentId, DateTime estimatedTimeOfArrival)
        {
            await _deliveryService.AssignDeliveryAgentAsync(orderId, agentId, estimatedTimeOfArrival);
            return Ok("Delivery agent assigned successfully");
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateDeliveryStatus(int id, [FromBody] string status)
        {
            await _deliveryService.UpdateDeliveryStatusAsync(id, status);
            return Ok("Delivery status updated successfully");
        }
        [HttpPatch("{orderId}/complete")]
        public async Task<IActionResult> CompleteDelivery(int orderId)
        {
            // Get the delivery by orderId
            var delivery = (await _deliveryService.GetAllDeliveriesAsync())
                .FirstOrDefault(d => d.OrderID == orderId);

            if (delivery == null)
                return NotFound("Delivery not found for the given order ID.");

            // Update delivery status to Completed
            delivery.Status = "Completed";
            await _deliveryService.UpdateDeliveryStatusAsync(delivery.DeliveryID, "Completed");

            // Set agent availability to true
            if (delivery.Agent != null)
            {
                delivery.Agent.IsAvailable = true;
                await _deliveryService.UpdateAgentAvailabilityAsync(delivery.Agent);
            }
            else
            {
                return BadRequest("No agent assigned to this delivery.");
            }

            return Ok("Delivery marked as completed and agent set to available.");
        }


    }
}
