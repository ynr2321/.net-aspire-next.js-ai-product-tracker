using Microsoft.AspNetCore.Mvc;
using AspireApp.ApiService.Models;
using AspireApp.ApiService.Data;

namespace AspireApp.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly List<User> _users;
    private readonly List<Order> _orders;

    public UsersController()
    {
        _users = MockData.GenerateUsers();
        _orders = MockData.GenerateOrders();
    }

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers()
    {
        return Ok(_users);
    }

    [HttpGet("{id}")]
    public ActionResult<User> GetUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public ActionResult<User> CreateUser(User user)
    {
        user.Id = _users.Max(u => u.Id) + 1;
        user.CreatedAt = DateTime.UtcNow;
        _users.Add(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, User user)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
            return NotFound();

        existingUser.Email = user.Email;
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Role = user.Role;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound();

        _users.Remove(user);
        return NoContent();
    }

    [HttpGet("{id}/orders")]
    public ActionResult<IEnumerable<Order>> GetUserOrders(int id)
    {
        var orders = _orders.Where(o => o.UserId == id).ToList();
        return Ok(orders);
    }
} 