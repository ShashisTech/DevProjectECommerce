using Microsoft.AspNetCore.Mvc;
using CatalogService.Data;
using CatalogService.Models;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly CatalogContext _context;

    public ProductsController(CatalogContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] int? categoryId, [FromQuery] string? name)
    {
        var query = _context.Products.AsQueryable();
        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);
        if (!string.IsNullOrEmpty(name))
            query = query.Where(p => p.Name.Contains(name));
        return Ok(query.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var p = _context.Products.Find(id);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    // Reserve stock for a product (decrement). Returns 200 with product when reserved, 404 if not found, 409 if insufficient stock.
    [HttpPost("{id}/reserve")]
    public IActionResult Reserve(int id, [FromBody] ReserveRequest req)
    {
        var p = _context.Products.Find(id);
        if (p == null) return NotFound();

        if (req.Quantity <= 0) return BadRequest(new { error = "Quantity must be greater than zero" });

        if (p.Stock < req.Quantity) return Conflict(new { error = "Insufficient stock" });

        p.Stock -= req.Quantity;
        _context.SaveChanges();

        return Ok(p);
    }
}
