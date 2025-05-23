using Microsoft.EntityFrameworkCore;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

namespace Online_food_delivery_system.Repository
{
    public class OrderRepository : IOrder
    {
        private readonly FoodDbContext _context;
        public OrderRepository(FoodDbContext context)
        {
            _context = context;
        }
        public async Task<Agent?> GetAvailableAgentAsync()
        {
            return await _context.Agents.FirstOrDefaultAsync(a => a.IsAvailable);
        }
        public async Task UpdateDeliveryAsync(Delivery delivery)
        {
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();
        }

        
        public async Task UpdateAgentAsync(Agent agent)
        {
            _context.Agents.Update(agent);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(Order order)
        {
            // Calculate the total amount for the order
            // Extract the list of ItemIDs from OrderMenuItems
            IList<int> orderMenuItemIds = order.OrderMenuItems.Select(o => o.ItemID).ToList();

            // Fetch the corresponding MenuItems from the database
            IEnumerable<MenuItem> menuItems = await _context.MenuItems
                .Where(m => orderMenuItemIds.Contains(m.ItemID))
                .ToListAsync();

            // Calculate the total amount
            order.TotalAmount = menuItems.Sum(m => m.Price ?? 0);

            // Add the order
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            //// Create the payment entry
            Delivery delivery = new Delivery
            {
                OrderID = order.OrderID,
                Status = "Pending",
                EstimatedTimeOfArrival = DateTime.Now.AddHours(1) // Example ETA
            };
            await _context.Deliveries.AddAsync(delivery);
            await _context.SaveChangesAsync();

            // Associate the delivery with the order
            order.Delivery = delivery;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int OrderID)
        {
            var ord = await _context.Orders.FindAsync(OrderID);
            if (ord != null)
            {
                _context.Orders.Remove(ord);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .Include(o => o.OrderMenuItems)
                .ThenInclude(omi => omi.MenuItem)
                .Include(o => o.Payment)
                .Include(o => o.Delivery)
                .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int? orderID)
        {
            return await _context.Orders
                .Include(o => o.Delivery)
                .ThenInclude(d => d.Agent) // Include the Agent navigation property
                .FirstOrDefaultAsync(o => o.OrderID == orderID);
        }

        public async Task UpdateAsync(Order order)
        {
            // Get the existing order from the database
            var existingOrder = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderID == order.OrderID);

            bool statusChangedToConfirmed = existingOrder != null &&
                                            existingOrder.Status != "Confirmed" &&
                                            order.Status == "Confirmed";

            // Update the order
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            // If status changed to Confirmed, create a payment
            if (statusChangedToConfirmed)
            {
                Payment payment = new Payment
                {
                    OrderID = order.OrderID,
                    Amount = order.TotalAmount,
                    PaymentMethod = "Google Pay", // or any default value
                    Status = "Pending"
                };

                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Optionally update payment if it already exists
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderID == order.OrderID);
                if (payment != null)
                {
                    payment.Amount = order.TotalAmount;
                    _context.Payments.Update(payment);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
