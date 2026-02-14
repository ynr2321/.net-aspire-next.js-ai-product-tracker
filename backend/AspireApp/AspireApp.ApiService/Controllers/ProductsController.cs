using Microsoft.AspNetCore.Mvc;
using AspireApp.ApiService.Models;
using AspireApp.ApiService.Data;

namespace AspireApp.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly List<Product> _products;

    public ProductsController()
    {
        _products = MockData.GenerateProducts();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        return Ok(_products);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetProduct(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public ActionResult<Product> CreateProduct(Product product)
    {
        product.Id = _products.Max(p => p.Id) + 1;
        product.CreatedAt = DateTime.UtcNow;
        _products.Add(product);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, Product product)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Id == id);
        if (existingProduct == null)
            return NotFound();

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.StockLevel = product.StockLevel;
        existingProduct.ReorderThreshold = product.ReorderThreshold;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        _products.Remove(product);
        return NoContent();
    }
} 