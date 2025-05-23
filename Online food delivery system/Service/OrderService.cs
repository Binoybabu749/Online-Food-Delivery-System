using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;

public class OrderService
{
    private readonly IOrder _orderRepository;
    public OrderService(IOrder orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int orderID)
    {
        return await _orderRepository.GetByIdAsync(orderID);
    }

    public async Task<Order> AddOrderAsync(Order order)
    {
       await _orderRepository.AddAsync(order);
        return order;
    }

    public async Task UpdateOrderStatusAsync(int orderId, string status)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null) throw new Exception("Order not found");
        order.Status = status;
        await _orderRepository.UpdateAsync(order);
    }

    public async Task<Order> UpdateOrderAsync(Order order)
    {
    //    order.CalculateTotalAmount();
        await _orderRepository.UpdateAsync(order);
        return order;
    }

    public async Task DeleteOrderAsync(int orderID)
    {
        await _orderRepository.DeleteAsync(orderID);
    }
   

    public async Task UpdateAgentAsync(Agent agent)
    {
        await _orderRepository.UpdateAgentAsync(agent);
    }

   
}
