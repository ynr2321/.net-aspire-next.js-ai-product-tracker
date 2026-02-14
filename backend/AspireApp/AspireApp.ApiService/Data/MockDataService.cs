using AspireApp.ApiService.Models;

namespace AspireApp.ApiService.Data;

public class MockDataService
{
    private static readonly List<Product> _products = MockData.GenerateProducts(100);
    private static readonly List<User> _users = MockData.GenerateUsers(30);
    private static readonly List<Order> _orders = MockData.GenerateOrders(500);

    public static List<Product> Products => _products;
    public static List<User> Users => _users;
    public static List<Order> Orders => _orders;
} 