using System;
using System.Collections.Generic;
using System.Linq;
using AspireApp.ApiService.Models;

namespace AspireApp.ApiService.Data;

public static class MockData
{
    private static readonly Random Random = new Random();

    public static List<Product> GenerateProducts(int count = 10)
    {
        var furnitureItems = new[]
        {
            new {
                Name = "Modern Dining Table Set",
                Description = "6-seater dining table with matching chairs, made from solid oak with a contemporary design",
                Price = 1299.99m,
                Category = "Dining Room"
            },
            new {
                Name = "L-Shaped Sectional Sofa",
                Description = "Spacious sectional sofa with premium fabric upholstery and memory foam cushions",
                Price = 2499.99m,
                Category = "Living Room"
            },
            new {
                Name = "Queen Platform Bed Frame",
                Description = "Minimalist platform bed frame with built-in storage and headboard",
                Price = 899.99m,
                Category = "Bedroom"
            },
            new {
                Name = "Coffee Table with Storage",
                Description = "Modern coffee table with hidden storage compartment and tempered glass top",
                Price = 449.99m,
                Category = "Living Room"
            },
            new {
                Name = "Bookshelf with Display Cabinet",
                Description = "5-tier bookshelf with glass-front display cabinet and adjustable shelves",
                Price = 599.99m,
                Category = "Living Room"
            },
            new {
                Name = "Console Table with Mirror",
                Description = "Entryway console table with integrated mirror and drawer storage",
                Price = 349.99m,
                Category = "Entryway"
            },
            new {
                Name = "Accent Armchair",
                Description = "Comfortable accent chair with premium velvet upholstery and solid wood frame",
                Price = 699.99m,
                Category = "Living Room"
            },
            new {
                Name = "TV Stand with Fireplace",
                Description = "Entertainment center with built-in electric fireplace and cable management",
                Price = 799.99m,
                Category = "Living Room"
            },
            new {
                Name = "Bar Cabinet",
                Description = "Stylish bar cabinet with glass doors, wine rack, and LED lighting",
                Price = 899.99m,
                Category = "Living Room"
            },
            new {
                Name = "Office Desk with Hutch",
                Description = "Executive desk with overhead storage and built-in power outlets",
                Price = 649.99m,
                Category = "Office"
            }
        };

        return Enumerable.Range(0, count).Select(i => new Product
        {
            Id = i + 1,
            Name = furnitureItems[i].Name,
            Description = furnitureItems[i].Description,
            Price = furnitureItems[i].Price,
            StockLevel = Random.Next(5, 50),
            ReorderThreshold = 10,
            Category = furnitureItems[i].Category,
            CreatedAt = DateTime.UtcNow.AddDays(-Random.Next(1, 365)),
            UpdatedAt = DateTime.UtcNow.AddDays(-Random.Next(0, 30))
        }).ToList();
    }

    public static List<User> GenerateUsers(int count = 30)
    {
        var firstNames = new[] { "John", "Jane", "Michael", "Emily", "David", "Sarah", "James", "Emma", "Robert", "Lisa" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
        var roles = new[] { "Admin", "Manager", "Customer", "Support" };
        
        return Enumerable.Range(1, count).Select(i => new User
        {
            Id = i,
            FirstName = firstNames[Random.Next(firstNames.Length)],
            LastName = lastNames[Random.Next(lastNames.Length)],
            Email = $"user{i}@example.com",
            Role = roles[Random.Next(roles.Length)],
            CreatedAt = DateTime.UtcNow.AddDays(-Random.Next(1, 365)),
            LastLogin = DateTime.UtcNow.AddDays(-Random.Next(0, 30))
        }).ToList();
    }

    public static List<Order> GenerateOrders(int count = 500)
    {
        var orders = new List<Order>();
        var states = new[] { "New York", "New Jersey", "Pennsylvania", "Connecticut" };
        var random = new Random();

        for (int i = 0; i < count; i++)
        {
            var orderDate = DateTime.UtcNow.AddDays(-random.Next(0, 30));
            var order = new Order
            {
                Id = i + 1,
                UserId = random.Next(1, 31),
                CreatedAt = orderDate,
                UpdatedAt = orderDate,
                TotalAmount = (decimal)Math.Round(random.NextDouble() * 1000, 2),
                State = states[random.Next(states.Length)],
                Status = GetRandomOrderStatus(random),
                Items = GenerateOrderItems(i + 1, GenerateProducts())
            };
            orders.Add(order);
        }

        return orders;
    }

    private static List<OrderItem> GenerateOrderItems(int orderId, List<Product> products)
    {
        var itemCount = Random.Next(1, 6); // 1-5 items per order
        return Enumerable.Range(1, itemCount).Select(i => new OrderItem
        {
            Id = (orderId * 100) + i,
            OrderId = orderId,
            ProductId = products[Random.Next(products.Count)].Id,
            Quantity = Random.Next(1, 5),
            UnitPrice = Math.Round((decimal)(Random.NextDouble() * 1000 + 10), 2)
        }).ToList();
    }

    private static string GetRandomOrderStatus(Random random)
    {
        var statuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
        return statuses[random.Next(statuses.Length)];
    }
} 