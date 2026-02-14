using Microsoft.AspNetCore.Mvc;
using AspireApp.ApiService.Models;
using AspireApp.ApiService.Data;

namespace AspireApp.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly List<Order> _orders;

    public OrdersController()
    {
        _orders = MockData.GenerateOrders();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Order>> GetOrders()
    {
        return Ok(_orders);
    }

    [HttpGet("{id}")]
    public ActionResult<Order> GetOrder(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpPost]
    public ActionResult<Order> CreateOrder(Order order)
    {
        order.Id = _orders.Max(o => o.Id) + 1;
        order.CreatedAt = DateTime.UtcNow;
        order.Status = "Pending";
        _orders.Add(order);
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateOrder(int id, Order order)
    {
        var existingOrder = _orders.FirstOrDefault(o => o.Id == id);
        if (existingOrder == null)
            return NotFound();

        existingOrder.Status = order.Status;
        existingOrder.ShippingAddress = order.ShippingAddress;
        existingOrder.TrackingNumber = order.TrackingNumber;
        existingOrder.UpdatedAt = DateTime.UtcNow;

        // Update status-specific timestamps
        if (order.Status == "Shipped" && existingOrder.Status != "Shipped")
            existingOrder.ShippedAt = DateTime.UtcNow;
        else if (order.Status == "Delivered" && existingOrder.Status != "Delivered")
            existingOrder.DeliveredAt = DateTime.UtcNow;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteOrder(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
            return NotFound();

        _orders.Remove(order);
        return NoContent();
    }

    [HttpGet("stats")]
    public ActionResult<object> GetOrderStats()
    {
        var stats = new
        {
            TotalOrders = _orders.Count,
            TotalRevenue = _orders.Sum(o => o.TotalAmount),
            OrdersByStatus = _orders
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() }),
            RecentOrders = _orders
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .Select(o => new
                {
                    o.Id,
                    o.Status,
                    o.TotalAmount,
                    o.CreatedAt,
                    UserName = o.User?.FirstName + " " + o.User?.LastName
                })
        };

        return Ok(stats);
    }
} 