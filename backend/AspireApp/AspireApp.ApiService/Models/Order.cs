namespace AspireApp.ApiService.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Status { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public string ShippingAddress { get; set; } = string.Empty;
    public string? TrackingNumber { get; set; }
} 